# Supabase Database Setup Guide

## 🔧 Configuration Steps

### 1. Update Connection String Password
Replace `[YOUR-PASSWORD]` in both files with your actual Supabase database password:
- `appsettings.json`
- `appsettings.Production.json`

Your connection string format:
```
Host=aws-0-ap-south-1.pooler.supabase.com;Port=5432;Database=postgres;Username=postgres.osytnqmhrsafbmgjvdlx;Password=YOUR_ACTUAL_PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

### 2. Database Migration Commands

Run these commands in your project directory:

```bash
# Create initial migration (if not exists)
dotnet ef migrations add InitialCreate

# Update database schema on Supabase
dotnet ef database update
```

### 3. Environment Configuration

The application will use different connection strings based on environment:
- **Development**: Uses `appsettings.Development.json` (falls back to `appsettings.json`)
- **Production**: Uses `appsettings.Production.json`

### 4. SSL Configuration

The connection string includes:
- `SSL Mode=Require` - Forces SSL connection
- `Trust Server Certificate=true` - Trusts the Supabase certificate

### 5. Database Schema

Your database will have these tables:
- `Users` - User accounts with authentication
- `Expenses` - Expense tracking linked to users

### 6. Real-time Data Storage

Once configured, all operations will:
- ✅ Store data directly in Supabase PostgreSQL
- ✅ Work in real-time with your production database
- ✅ Support all CRUD operations (Create, Read, Update, Delete)
- ✅ Maintain data relationships and constraints

## 🚀 Deployment Checklist

- [ ] Replace `[YOUR-PASSWORD]` with actual password
- [ ] Run `dotnet ef database update`
- [ ] Test connection with `dotnet run`
- [ ] Verify data is being stored in Supabase dashboard
- [ ] Set environment to Production for deployment

## 🔍 Troubleshooting

If you encounter connection issues:
1. Check your Supabase password is correct
2. Verify your Supabase project is active
3. Ensure your IP is allowed in Supabase settings
4. Check SSL/TLS settings

## 📊 Monitoring

Monitor your database activity in:
- Supabase Dashboard → Database → Logs
- Supabase Dashboard → Database → Tables
