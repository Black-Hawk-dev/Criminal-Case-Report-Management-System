#!/bin/bash

echo "🚀 بدء تشغيل نظام إدارة التحقيقات والبلاغات الجنائية"
echo "=============================================="

# التحقق من وجود .NET SDK
if ! command -v dotnet &> /dev/null; then
    echo "❌ .NET SDK غير مثبت. يرجى تثبيت .NET 8.0 SDK أولاً"
    exit 1
fi

echo "✅ تم العثور على .NET SDK"

# استعادة الحزم
echo "📦 استعادة الحزم..."
dotnet restore

if [ $? -ne 0 ]; then
    echo "❌ فشل في استعادة الحزم"
    exit 1
fi

# بناء المشروع
echo "🔨 بناء المشروع..."
dotnet build

if [ $? -ne 0 ]; then
    echo "❌ فشل في بناء المشروع"
    exit 1
fi

echo "✅ تم بناء المشروع بنجاح"

# تشغيل المشروع
echo "🌐 تشغيل المشروع..."
echo "📍 الرابط: http://localhost:5000"
echo "🔐 بيانات الدخول الافتراضية:"
echo "   اسم المستخدم: admin"
echo "   كلمة المرور: admin123"
echo ""
echo "⚠️  للإيقاف: اضغط Ctrl+C"
echo "=============================================="

dotnet run --urls=http://localhost:5000