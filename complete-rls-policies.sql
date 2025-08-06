-- COMPLETE RLS POLICIES FOR FINANCEAPP
-- Copy this entire file into your Supabase SQL Editor

-- =====================================
-- ENABLE ROW LEVEL SECURITY
-- =====================================

-- Enable RLS on both tables
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;
ALTER TABLE public."Expenses" ENABLE ROW LEVEL SECURITY;

-- =====================================
-- USERS TABLE POLICIES
-- =====================================

-- Policy 1: Users can only view their own profile
CREATE POLICY "users_select_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- User can only see their own record
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy 2: Users can update their own profile
CREATE POLICY "users_update_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR UPDATE
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
)
WITH CHECK (
    -- Prevent users from changing their ID or role
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
    AND "Role" = OLD."Role" -- Prevent role escalation
);

-- Policy 3: Allow user registration (no authentication required)
CREATE POLICY "users_insert_registration"
ON "public"."Users"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (
    -- New users can only register with 'User' role
    "Role" = 'User' OR "Role" IS NULL
);

-- Policy 4: Prevent users from deleting their account (security)
-- Comment this out if you want to allow account deletion
CREATE POLICY "users_no_delete"
ON "public"."Users"
AS RESTRICTIVE
FOR DELETE
TO public
USING (false);

-- Alternative Policy 4: Allow users to delete their own account
-- Uncomment this if you want to allow self-deletion
/*
CREATE POLICY "users_delete_own_account"
ON "public"."Users"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);
*/

-- =====================================
-- EXPENSES TABLE POLICIES
-- =====================================

-- Policy 1: Users can only view their own expenses
CREATE POLICY "expenses_select_own"
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy 2: Users can only create expenses for themselves
CREATE POLICY "expenses_insert_own"
ON "public"."Expenses"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy 3: Users can only update their own expenses
CREATE POLICY "expenses_update_own"
ON "public"."Expenses"
AS PERMISSIVE
FOR UPDATE
TO public
USING (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
)
WITH CHECK (
    -- Prevent changing ownership of expenses
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy 4: Users can only delete their own expenses
CREATE POLICY "expenses_delete_own"
ON "public"."Expenses"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- =====================================
-- ADMIN OVERRIDE POLICIES (OPTIONAL)
-- =====================================

-- Admin Policy 1: Admins can view all users
CREATE POLICY "admin_select_all_users"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- Admin Policy 2: Admins can update any user
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

-- Admin Policy 3: Admins can delete any user
CREATE POLICY "admin_delete_any_user"
ON "public"."Users"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- Admin Policy 4: Admins can view all expenses
CREATE POLICY "admin_select_all_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- Admin Policy 5: Admins can modify any expense
CREATE POLICY "admin_modify_any_expense"
ON "public"."Expenses"
AS PERMISSIVE
FOR ALL
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
)
WITH CHECK (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- =====================================
-- TESTING AND VERIFICATION
-- =====================================

-- Test Policy (REMOVE after testing)
-- This temporary policy helps debug JWT claims
CREATE POLICY "test_debug_claims"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- Temporarily allow all to test JWT structure
    true -- IMPORTANT: Remove this after confirming JWT works
);

-- =====================================
-- ALTERNATIVE JWT CLAIM PATHS
-- =====================================

-- If the above policies don't work, try these alternatives:

/*
-- Alternative using auth.jwt() instead of current_setting
CREATE POLICY "users_select_alt"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    "Id" = (auth.jwt() ->> 'nameid')::integer OR
    "Id" = (auth.jwt() ->> 'sub')::integer OR
    "Id" = (auth.jwt() ->> 'user_id')::integer
);

CREATE POLICY "expenses_select_alt"
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    "UserId" = (auth.jwt() ->> 'nameid')::integer OR
    "UserId" = (auth.jwt() ->> 'sub')::integer OR
    "UserId" = (auth.jwt() ->> 'user_id')::integer
);
*/

-- =====================================
-- NOTES AND INSTRUCTIONS
-- =====================================

/*
SETUP INSTRUCTIONS:
1. Copy this entire file
2. Go to Supabase Dashboard → SQL Editor
3. Paste and execute all policies
4. Test with your application
5. Remove the "test_debug_claims" policy after testing

SECURITY FEATURES:
✅ Users can only see/modify their own data
✅ Admin users have full access
✅ New users can register with 'User' role only
✅ Prevents role escalation
✅ Prevents expense ownership changes
✅ Account deletion is restricted (can be enabled)

JWT CLAIMS USED:
- 'nameid' → User ID (from ClaimTypes.NameIdentifier)
- 'role' → User Role (from ClaimTypes.Role)
- 'name' → Username (from ClaimTypes.Name) 
- 'email' → Email (from ClaimTypes.Email)

TESTING:
After setup, test that:
1. Users can only see their own expenses
2. Users can only update their own profile
3. Admin users can see all data
4. Registration works for new users
5. Security is enforced at database level
*/
