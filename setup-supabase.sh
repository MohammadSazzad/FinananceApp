#!/bin/bash

# Supabase Database Setup Script
echo "🚀 FinanceApp Supabase Setup"
echo "================================"

# Check if password is still placeholder
if grep -q "\[YOUR-PASSWORD\]" appsettings.json; then
    echo "❌ ERROR: Please replace [YOUR-PASSWORD] with your actual Supabase password in:"
    echo "   - appsettings.json"
    echo "   - appsettings.Production.json"
    echo ""
    echo "Then run this script again."
    exit 1
fi

echo "✅ Password configured"

# Test database connection
echo "🔍 Testing database connection..."
dotnet ef database drop --force --dry-run 2>/dev/null
if [ $? -eq 0 ]; then
    echo "✅ Database connection successful"
else
    echo "❌ Database connection failed. Please check your connection string."
    exit 1
fi

# Create migration if needed
if [ ! -d "Migrations" ]; then
    echo "📝 Creating initial migration..."
    dotnet ef migrations add InitialCreate
    if [ $? -ne 0 ]; then
        echo "❌ Failed to create migration"
        exit 1
    fi
    echo "✅ Migration created"
else
    echo "✅ Migrations already exist"
fi

# Update database
echo "🗄️  Updating database schema..."
dotnet ef database update
if [ $? -eq 0 ]; then
    echo "✅ Database updated successfully!"
    echo ""
    echo "🎉 Setup complete! Your app is now connected to Supabase."
    echo ""
    echo "Next steps:"
    echo "1. Run 'dotnet run' to start your application"
    echo "2. Check your Supabase dashboard to see the tables"
    echo "3. Test user registration and expense creation"
else
    echo "❌ Database update failed"
    exit 1
fi
