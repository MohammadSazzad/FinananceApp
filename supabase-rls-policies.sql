-- Row Level Security (RLS) Policies for FinanceApp
-- Run these commands in your Supabase SQL Editor

-- =====================================
-- ENABLE RLS ON TABLES
-- =====================================

-- Enable RLS on Users table
ALTER TABLE public."Users" ENABLE ROW LEVEL SECURITY;

-- Enable RLS on Expenses table  
ALTER TABLE public."Expenses" ENABLE ROW LEVEL SECURITY;

-- =====================================
-- USERS TABLE POLICIES
-- =====================================

-- Policy: Users can only view their own profile
CREATE POLICY "users_select_own_profile" 
ON public."Users"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- User can only see their own record
    auth.uid()::text = "Id"::text
);

-- Policy: Users can update their own profile
CREATE POLICY "users_update_own_profile"
ON public."Users"
AS PERMISSIVE 
FOR UPDATE
TO public
USING (auth.uid()::text = "Id"::text)
WITH CHECK (auth.uid()::text = "Id"::text);

-- Policy: Allow user registration (INSERT)
CREATE POLICY "users_insert_registration"
ON public."Users"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (true);

-- =====================================
-- EXPENSES TABLE POLICIES
-- =====================================

-- Policy: Users can only view their own expenses
CREATE POLICY "expenses_select_own_expenses"
ON public."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    -- User can only see expenses where UserId matches their authenticated user ID
    auth.uid()::text = "UserId"::text
);

-- Policy: Users can insert their own expenses
CREATE POLICY "expenses_insert_own_expenses"
ON public."Expenses"
AS PERMISSIVE
FOR INSERT
TO public
WITH CHECK (
    -- User can only create expenses for themselves
    auth.uid()::text = "UserId"::text
);

-- Policy: Users can update their own expenses
CREATE POLICY "expenses_update_own_expenses"
ON public."Expenses"
AS PERMISSIVE
FOR UPDATE
TO public
USING (auth.uid()::text = "UserId"::text)
WITH CHECK (auth.uid()::text = "UserId"::text);

-- Policy: Users can delete their own expenses
CREATE POLICY "expenses_delete_own_expenses"
ON public."Expenses"
AS PERMISSIVE
FOR DELETE
TO public
USING (
    -- User can only delete their own expenses
    auth.uid()::text = "UserId"::text
);

-- =====================================
-- ALTERNATIVE APPROACH (If using custom authentication)
-- =====================================

-- If you're using custom JWT authentication instead of Supabase Auth,
-- you might need to extract user ID from JWT claims:

/*
-- Example using JWT claims (uncomment if needed)
CREATE OR REPLACE FUNCTION get_current_user_id()
RETURNS TEXT AS $$
BEGIN
    -- Extract user ID from JWT claims
    RETURN auth.jwt() ->> 'sub';
END;
$$ LANGUAGE plpgsql SECURITY DEFINER;

-- Then use in policies like:
CREATE POLICY "expenses_select_custom_auth"
ON public."Expenses"
AS PERMISSIVE
FOR SELECT
TO public
USING (
    get_current_user_id() = "UserId"::text
);
*/

-- =====================================
-- ADMIN OVERRIDE POLICIES (OPTIONAL)
-- =====================================

-- Policy: Admin users can view all expenses (optional)
CREATE POLICY "expenses_admin_all_access"
ON public."Expenses" 
AS PERMISSIVE
FOR ALL
TO public
USING (
    -- Check if user has admin role in JWT claims
    (auth.jwt() ->> 'role') = 'Admin'
);

-- Policy: Admin users can view all users (optional)
CREATE POLICY "users_admin_all_access"
ON public."Users"
AS PERMISSIVE
FOR ALL  
TO public
USING (
    -- Check if user has admin role in JWT claims
    (auth.jwt() ->> 'role') = 'Admin'
);

-- =====================================
-- VERIFICATION QUERIES
-- =====================================

-- Test RLS policies (run after creating policies)
-- These should only return data for the authenticated user

-- Test expenses access
-- SELECT * FROM public."Expenses";

-- Test users access  
-- SELECT * FROM public."Users";

-- =====================================
-- NOTES
-- =====================================

/*
1. auth.uid() - Returns the authenticated user's UUID from Supabase Auth
2. auth.jwt() - Returns the full JWT payload
3. These policies assume your application sets the user context properly
4. For custom authentication, you may need to modify the auth functions
5. The policies are permissive (allow access) rather than restrictive
6. Admin policies are optional - remove if not needed

IMPORTANT: 
- Make sure your application properly authenticates users
- Test the policies thoroughly before going to production
- Consider adding logging for security audit trails
*/
