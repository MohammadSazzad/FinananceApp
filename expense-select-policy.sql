-- CORRECT RLS POLICY FOR YOUR FINANCEAPP
-- Copy this into your Supabase SQL Editor

-- Enable RLS on the Expenses table
ALTER TABLE public."Expenses" ENABLE ROW LEVEL SECURITY;

-- Policy: Users can only view their own expenses
CREATE POLICY "users_can_view_own_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- Your JWT uses ClaimTypes.NameIdentifier which maps to 'nameid' in JWT
    -- This matches with your UserService.cs JWT generation
    "UserId" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- ALTERNATIVE: If Supabase uses different JWT claim structure
CREATE POLICY "users_can_view_own_expenses_alt"
ON "public"."Expenses" 
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- Alternative claim paths that might work
    "UserId" = (auth.jwt() ->> 'nameid')::integer OR
    "UserId" = (auth.jwt() ->> 'sub')::integer OR  
    "UserId" = (auth.jwt() ->> 'user_id')::integer
);

-- TESTING POLICY (Use this to test your setup)
-- This policy allows you to verify the JWT structure
CREATE POLICY "test_jwt_claims" 
ON "public"."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- This will help you see what claims are available
    -- Check Supabase logs to see the actual JWT structure
    true -- Temporarily allow all (REMOVE after testing)
);

-- =====================================
-- USERS TABLE RLS POLICIES
-- =====================================

-- Enable RLS on the Users table
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;

-- Policy: Users can only view their own profile
CREATE POLICY "users_can_view_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- User can only see their own record
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy: Users can update their own profile
CREATE POLICY "users_can_update_own_profile"
ON "public"."Users"
AS PERMISSIVE
FOR UPDATE
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
)
WITH CHECK (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);

-- Policy: Allow user registration (INSERT)
-- This allows new users to register without being authenticated
CREATE POLICY "users_can_register"
ON "public"."Users"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (true);

-- Policy: Users cannot delete their own account (optional security)
-- Remove this policy if you want to allow account deletion
CREATE POLICY "users_cannot_delete_account"
ON "public"."Users"
AS RESTRICTIVE
FOR DELETE
TO public
USING (false);

-- Alternative: Allow users to delete their own account
-- Uncomment this if you want to allow account deletion
/*
CREATE POLICY "users_can_delete_own_account"
ON "public"."Users"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    "Id" = (current_setting('request.jwt.claims')::json ->> 'nameid')::integer
);
*/

-- =====================================
-- ADMIN OVERRIDE POLICIES (OPTIONAL)
-- =====================================

-- Policy: Admin users can view all users
CREATE POLICY "admin_can_view_all_users"
ON "public"."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- Check if user has admin role in JWT claims
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);

-- Policy: Admin users can update any user
CREATE POLICY "admin_can_update_any_user"
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

-- Policy: Admin users can view all expenses
CREATE POLICY "admin_can_view_all_expenses"
ON "public"."Expenses"
AS PERMISSIVE
FOR ALL
TO public
USING (
    (current_setting('request.jwt.claims')::json ->> 'role') = 'Admin'
);
