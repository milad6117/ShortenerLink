# مرحله اول: مرحله Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /app

# کپی کردن فایل‌های پروژه و Restore کردن وابستگی‌ها
COPY *.csproj ./
RUN dotnet restore

# کپی کردن بقیه فایل‌ها و Build کردن پروژه
COPY . ./
RUN dotnet publish -c Release -o out

# مرحله دوم: مرحله نهایی (Runtime)
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app

# کپی کردن خروجی مرحله قبل به این مرحله
COPY --from=build-env /app/out .

# تنظیم پورت و اجرای برنامه
EXPOSE 8080
ENTRYPOINT ["dotnet", "YourProjectName.dll"]