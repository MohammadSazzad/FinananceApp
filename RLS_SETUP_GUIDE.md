# RLS Setup Guide for FinanceApp

## 🎯 **Summary: Where to Configure What**

### **SUPABASE ONLY** (Database Side)
- ✅ Set RLS policies in Supabase SQL Editor
- ✅ Enable Row Level Security on tables
- ✅ Configure JWT settings (if using custom auth)

### **ASP.NET CORE** (Application Side) 
- ❌ No RLS policy code needed
- ✅ Ensure proper JWT claims structure
- ✅ Configure database connection with user context

---

## 📊 **Step-by-Step Setup**

### **1. Supabase Dashboard Setup**

#### **A. Enable RLS on Tables**
Go to **Supabase → Database → Tables** and run in SQL Editor:

```sql
-- Enable RLS
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."Expenses" ENABLE ROW LEVEL SECURITY;
```

#### **B. Create RLS Policies**
In **Supabase → SQL Editor**, run:

```sql
-- Policy for Expenses (Users can only see their own expenses)
CREATE POLICY "users_view_own_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- Match UserId with authenticated user from JWT
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy for INSERT (Users can only create expenses for themselves)
CREATE POLICY "users_insert_own_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy for UPDATE (Users can only update their own expenses)
CREATE POLICY "users_update_own_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR UPDATE
TO public
USING ("UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer)
WITH CHECK ("UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer);

-- Policy for DELETE (Users can only delete their own expenses)
CREATE POLICY "users_delete_own_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);
```

#### **C. Configure JWT Settings (If Using Custom Auth)**
In **Supabase → Settings → API**, you might need to configure JWT settings if not using Supabase Auth.

### **2. ASP.NET Core Configuration (Your Project)**

#### **A. Update JWT Claims Structure**
Your current JWT is perfect! It includes:
- `ClaimTypes.NameIdentifier` (maps to 'nameid' in JWT)
- `ClaimTypes.Name` (username)
- `ClaimTypes.Email` (email)
- `ClaimTypes.Role` (user role)

#### **B. Database Context Configuration (Already Done)**
Your `Program.cs` is already configured correctly with connection retry logic.

#### **C. Optional: Add User Context Middleware**
Create this middleware to pass user context to database queries:

```csharp
// Add this to your Program.cs if needed for advanced scenarios
app.Use(async (context, next) =>
{
    var token = context.Session.GetString("JWTToken");
    if (!string.IsNullOrEmpty(token))
    {
        // Set user context for database queries
        context.Items["UserToken"] = token;
    }
    await next();
});
```

---

## 🔑 **How It Works**

### **Data Flow:**
1. **User logs in** → ASP.NET Core generates JWT with user claims
2. **JWT stored in session** → Available for database operations
3. **Database query executed** → Supabase reads JWT claims
4. **RLS policy checks** → `nameid` claim compared with `UserId`
5. **Data filtered** → User only sees their own expenses

### **Security Benefits:**
✅ **Database-level security** - Even if app logic fails, database protects data
✅ **No bypass possible** - RLS enforced at PostgreSQL level
✅ **Multi-tenant ready** - Each user completely isolated
✅ **Performance optimized** - Policies use indexes efficiently

---

## 🚀 **Testing Your Setup**

### **1. Test RLS Policies**
In Supabase SQL Editor:

```sql
-- This should return no rows (anonymous user)
SELECT * FROM public."Expenses";

-- Enable specific user context for testing
SET request.jwt.claims = '{"nameid": "1", "name": "testuser"}';
SELECT * FROM public."Expenses";
```

### **2. Test from Your App**
1. Run your app: `dotnet run`
2. Register/login a user
3. Create some expenses
4. Verify only that user's expenses are visible

### **3. Verify Security**
Try to access another user's data - should be blocked by RLS.

---

## 💡 **Key Points**

- **RLS policies = Supabase only**
- **JWT configuration = Your ASP.NET Core app**
- **No C# RLS code needed**
- **Database enforces security automatically**
- **Works with your existing authentication**

The beauty of RLS is that it's **transparent to your application code** - you write normal queries, but the database automatically filters results based on the authenticated user!
