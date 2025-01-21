# E-Commerce Project

## **متطلبات التشغيل**
لضمان تشغيل المشروع بنجاح على جهازك المحلي، تأكد من توافر العناصر التالية:

1. **Node.js** (لإدارة الخادم)
2. **MySQL** (أو أي قاعدة بيانات تستخدمها)
3. **Git** (لإدارة المستودع)
4. **Postman** أو متصفح لتجربة الـ APIs (اختياري)

---

## **خطوات إعداد المشروع محليًا و إعداد Docker**


### **1. استنساخ المستودع**
قم بنسخ المستودع من GitHub باستخدام الأمر التالي:
```bash
git clone https://github.com/Hussien816/Snowwhite-ecommerce.git
استبدل الرابط بعنوان المستودع الخاص بك.

### **2. تثبيت التبعيات**
تأكد من تثبيت التبعيات باستخدام الأمر:
```bash
npm install
```

### **3. إعداد Docker**
لإعداد المشروع على Heroku، ستحتاج إلى إعداد Docker. 
قم بإنشاء ملف Dockerfile في جذر المشروع الخاص بك. 

مثال على محتوى Dockerfile:
```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 5000

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "اسم_التطبيق.dll"]
```

قم بتشغيل الخادم باستخدام الأمر:
```bash
node server.js
```

### **4. تشغيل الخادم**
قم بتشغيل الخادم باستخدام الأمر:
```bash
node server.js
```

يمكنك الوصول إلى واجهة برمجة التطبيقات عبر الرابط:
```
http://localhost:5037/api
