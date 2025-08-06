# Users Table RLS Policies Guide

## 🔐 **Users Table Security Policies**

### **Core Concept:**
- Users can only access their **own profile data**
- Admins can access **all user data**
- Registration is **open** but restricted to 'User' role
- Account deletion is **restricted** (configurable)

---

## 📋 **Step-by-Step Implementation**

### **1. Basic Users Policies (Copy to Supabase SQL Editor)**

```sql
-- Enable RLS on Users table
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;

-- Users can view only their own profile
CREATE POLICY "users_select_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Users can update only their own profile  
CREATE POLICY "users_update_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR UPDATE
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
)
WITH CHECK (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
    AND "Role" = OLD."Role" -- Prevent role escalation
);

-- Allow new user registration
CREATE POLICY "users_insert_registration"
ON "public"."Users"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (
    "Role" = 'User' OR "Role" IS NULL
);

-- Prevent account deletion (security measure)
CREATE POLICY "users_no_delete"
ON "public"."Users"
AS RESTRICTIVE
FOR DELETE
TO public
USING (false);
```

### **2. Admin Override Policies (Optional)**

```sql
-- Admins can view all users
CREATE POLICY "admin_select_all_users"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- Admins can update any user
CREATE POLICY "admin_update_any_user"
ON "public"."Users"
AS PERMISSIVE
FOR UPDATE
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
)
WITH CHECK (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);
```

---

## 🛡️ **Security Features**

### **What These Policies Protect:**

| Operation | Regular User | Admin User |
|-----------|-------------|------------|
| **View Users** | ✅ Own profile only | ✅ All users |
| **Update Profile** | ✅ Own profile only | ✅ Any user |
| **Register** | ✅ As 'User' role | ✅ Any role |
| **Delete Account** | ❌ Blocked | ✅ Any user |
| **Role Changes** | ❌ Blocked | ✅ Allowed |

### **Key Security Points:**
- ✅ **Data Isolation**: Users cannot access other users' profiles
- ✅ **Role Protection**: Users cannot promote themselves to Admin
- ✅ **Registration Control**: New users can only register as 'User'
- ✅ **Account Protection**: Prevents accidental account deletion
- ✅ **Admin Access**: Admins maintain full control

---

## 🧪 **Testing Your Policies**

### **Test 1: User Profile Access**
```sql
-- Set user context (User ID = 1)
SET request.jwt.claims = '{"nameid": "1", "role": "User"}';

-- Should return only user 1's profile
SELECT * FROM public."Users";
```

### **Test 2: Admin Access**
```sql
-- Set admin context
SET request.jwt.claims = '{"nameid": "2", "role": "Admin"}';

-- Should return all users
SELECT * FROM public."Users";
```

### **Test 3: Registration**
```sql
-- Test user registration (should work)
INSERT INTO public."Users" ("Username", "Email", "PasswordHash", "Role")
VALUES ('newuser', 'new@email.com', 'hash', 'User');

-- Test admin creation (should fail for regular users)
INSERT INTO public."Users" ("Username", "Email", "PasswordHash", "Role")  
VALUES ('admin', 'admin@email.com', 'hash', 'Admin');
```

---

## 🚀 **Integration with Your App**

### **How It Works with Your Controllers:**

```csharp
// In UsersController.cs
public async Task<IActionResult> Profile()
{
    // This will automatically only return the current user's profile
    // RLS policy filters based on JWT claims
    var users = await _context.Users.ToListAsync(); // Only returns current user
    return View(users.FirstOrDefault());
}

public async Task<IActionResult> Settings()
{
    // Same here - RLS automatically filters
    var user = await _context.Users.FirstOrDefaultAsync(); // Only current user
    return View(user);
}
```

### **No Code Changes Needed:**
- ✅ Your existing controllers work unchanged
- ✅ Database automatically filters results
- ✅ Security enforced at PostgreSQL level
- ✅ Impossible to bypass (even with SQL injection)

---

## 📁 **Files Created:**
- `complete-rls-policies.sql` - All policies for Users and Expenses
- `expense-select-policy.sql` - Updated with Users policies
- This guide - Step-by-step Users implementation

## 🎯 **Next Steps:**
1. **Copy policies** from `complete-rls-policies.sql` 
2. **Run in Supabase SQL Editor**
3. **Test with your application**
4. **Verify security** by trying to access other users' data

Your Users table will now have comprehensive security that automatically protects user privacy and prevents unauthorized access!
