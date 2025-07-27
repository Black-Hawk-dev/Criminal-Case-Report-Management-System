# مخطط قاعدة البيانات - ERD
## Criminal Case Management System Database Schema

## 📊 الجداول الرئيسية

### 1. Users (المستخدمين)
```sql
Users {
    Id INT PRIMARY KEY
    FullName NVARCHAR(100) NOT NULL
    Username NVARCHAR(50) UNIQUE NOT NULL
    PasswordHash NVARCHAR(255) NOT NULL
    Role INT NOT NULL -- 1=SystemAdmin, 2=Investigator, 3=ReportWriter
    Rank NVARCHAR(20)
    Department NVARCHAR(100)
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
    IsActive BIT DEFAULT 1
}
```

### 2. Reports (البلاغات)
```sql
Reports {
    Id INT PRIMARY KEY
    ReporterName NVARCHAR(100) NOT NULL
    ReporterIdNumber NVARCHAR(20) NOT NULL
    Type INT NOT NULL -- 1=Theft, 2=Assault, 3=Fraud, 4=Murder, 5=Robbery, 6=Other
    Description NVARCHAR(1000) NOT NULL
    ReportDate DATETIME DEFAULT CURRENT_TIMESTAMP
    Location NVARCHAR(200)
    IsActive BIT DEFAULT 1
    CreatedByUserId INT NOT NULL
    
    FOREIGN KEY (CreatedByUserId) REFERENCES Users(Id)
}
```

### 3. Cases (القضايا)
```sql
Cases {
    Id INT PRIMARY KEY
    CaseNumber NVARCHAR(50) UNIQUE NOT NULL
    Title NVARCHAR(200) NOT NULL
    Description NVARCHAR(1000)
    CreatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
    ClosedDate DATETIME
    Status INT DEFAULT 1 -- 1=UnderInvestigation, 2=Closed, 3=TransferredToProsecution, 4=Suspended
    Notes NVARCHAR(500)
    ReportId INT NOT NULL
    AssignedInvestigatorId INT NOT NULL
    
    FOREIGN KEY (ReportId) REFERENCES Reports(Id)
    FOREIGN KEY (AssignedInvestigatorId) REFERENCES Users(Id)
}
```

### 4. Suspects (المتهمين)
```sql
Suspects {
    Id INT PRIMARY KEY
    FullName NVARCHAR(100) NOT NULL
    IdNumber NVARCHAR(20) UNIQUE NOT NULL
    DateOfBirth DATETIME NOT NULL
    Address NVARCHAR(200)
    PhoneNumber NVARCHAR(20)
    Gender NVARCHAR(10)
    Nationality NVARCHAR(50)
    Notes NVARCHAR(500)
    CreatedAt DATETIME DEFAULT CURRENT_TIMESTAMP
    IsActive BIT DEFAULT 1
}
```

### 5. Attachments (المرفقات)
```sql
Attachments {
    Id INT PRIMARY KEY
    FileName NVARCHAR(255) NOT NULL
    FilePath NVARCHAR(500) NOT NULL
    FileType NVARCHAR(100)
    FileSize BIGINT NOT NULL
    Description NVARCHAR(200)
    UploadedAt DATETIME DEFAULT CURRENT_TIMESTAMP
    ReportId INT -- Nullable for polymorphic relationship
    CaseId INT   -- Nullable for polymorphic relationship
    
    FOREIGN KEY (ReportId) REFERENCES Reports(Id)
    FOREIGN KEY (CaseId) REFERENCES Cases(Id)
}
```

### 6. SuspectReports (ربط المتهمين بالبلاغات)
```sql
SuspectReports {
    Id INT PRIMARY KEY
    SuspectId INT NOT NULL
    ReportId INT NOT NULL
    AssociatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
    Notes NVARCHAR(500)
    
    FOREIGN KEY (SuspectId) REFERENCES Suspects(Id)
    FOREIGN KEY (ReportId) REFERENCES Reports(Id)
    UNIQUE(SuspectId, ReportId)
}
```

### 7. SuspectCases (ربط المتهمين بالقضايا)
```sql
SuspectCases {
    Id INT PRIMARY KEY
    SuspectId INT NOT NULL
    CaseId INT NOT NULL
    AssociatedDate DATETIME DEFAULT CURRENT_TIMESTAMP
    Notes NVARCHAR(500)
    Status NVARCHAR(50) -- 'متهم رئيسي', 'متهم ثانوي', 'شاهد'
    
    FOREIGN KEY (SuspectId) REFERENCES Suspects(Id)
    FOREIGN KEY (CaseId) REFERENCES Cases(Id)
    UNIQUE(SuspectId, CaseId)
}
```

## 🔗 العلاقات (Relationships)

### One-to-Many Relationships:
1. **Users → Reports**: مستخدم واحد يمكنه إنشاء عدة بلاغات
2. **Users → Cases**: محقق واحد يمكنه إدارة عدة قضايا
3. **Reports → Cases**: بلاغ واحد يمكن أن يرتبط بعدة قضايا
4. **Reports → Attachments**: بلاغ واحد يمكن أن يحتوي على عدة مرفقات
5. **Cases → Attachments**: قضية واحدة يمكن أن تحتوي على عدة مرفقات

### Many-to-Many Relationships:
1. **Suspects ↔ Reports**: متهم يمكن أن يرتبط بعدة بلاغات، وبلاغ يمكن أن يرتبط بعدة متهمين
2. **Suspects ↔ Cases**: متهم يمكن أن يرتبط بعدة قضايا، وقضية يمكن أن ترتبط بعدة متهمين

## 📈 مخطط العلاقات المرئي

```
Users (1) ----< Reports (M)
  |               |
  |               |
  |               v
  |           Cases (M)
  |               |
  |               |
  v               v
Cases (M) ----< Attachments (M) >---- Reports (M)
  |                                       |
  |                                       |
  v                                       v
SuspectCases (M) >---- Suspects ----< SuspectReports (M)
```

## 🎯 الفهارس المقترحة

### فهارس الأداء:
```sql
-- فهارس فريدة
CREATE UNIQUE INDEX IX_Users_Username ON Users(Username);
CREATE UNIQUE INDEX IX_Cases_CaseNumber ON Cases(CaseNumber);
CREATE UNIQUE INDEX IX_Suspects_IdNumber ON Suspects(IdNumber);

-- فهارس البحث
CREATE INDEX IX_Reports_ReporterName ON Reports(ReporterName);
CREATE INDEX IX_Reports_ReporterIdNumber ON Reports(ReporterIdNumber);
CREATE INDEX IX_Reports_Type ON Reports(Type);
CREATE INDEX IX_Reports_ReportDate ON Reports(ReportDate);
CREATE INDEX IX_Cases_Status ON Cases(Status);
CREATE INDEX IX_Cases_CreatedDate ON Cases(CreatedDate);
CREATE INDEX IX_Suspects_FullName ON Suspects(FullName);

-- فهارس المفاتيح الخارجية
CREATE INDEX IX_Reports_CreatedByUserId ON Reports(CreatedByUserId);
CREATE INDEX IX_Cases_ReportId ON Cases(ReportId);
CREATE INDEX IX_Cases_AssignedInvestigatorId ON Cases(AssignedInvestigatorId);
CREATE INDEX IX_Attachments_ReportId ON Attachments(ReportId);
CREATE INDEX IX_Attachments_CaseId ON Attachments(CaseId);
```

## 🔐 قيود الأمان

### قيود التكامل المرجعي:
- **ON DELETE RESTRICT**: للعلاقات الحرجة (Users → Reports, Users → Cases)
- **ON DELETE CASCADE**: للعلاقات التابعة (Reports → Attachments, Cases → Attachments)

### قيود التحقق:
```sql
-- التحقق من صحة الأدوار
ALTER TABLE Users ADD CONSTRAINT CK_Users_Role 
CHECK (Role IN (1, 2, 3));

-- التحقق من صحة نوع البلاغ
ALTER TABLE Reports ADD CONSTRAINT CK_Reports_Type 
CHECK (Type IN (1, 2, 3, 4, 5, 6));

-- التحقق من صحة حالة القضية
ALTER TABLE Cases ADD CONSTRAINT CK_Cases_Status 
CHECK (Status IN (1, 2, 3, 4));

-- التحقق من تاريخ الإغلاق
ALTER TABLE Cases ADD CONSTRAINT CK_Cases_ClosedDate 
CHECK (ClosedDate IS NULL OR ClosedDate >= CreatedDate);
```

## 📊 الإحصائيات والتقارير

### استعلامات الإحصائيات الأساسية:
```sql
-- إجمالي البلاغات النشطة
SELECT COUNT(*) FROM Reports WHERE IsActive = 1;

-- إجمالي القضايا حسب الحالة
SELECT Status, COUNT(*) FROM Cases GROUP BY Status;

-- البلاغات حسب النوع
SELECT Type, COUNT(*) FROM Reports WHERE IsActive = 1 GROUP BY Type;

-- المحققين وعدد القضايا المعينة لهم
SELECT u.FullName, COUNT(c.Id) as CaseCount
FROM Users u
LEFT JOIN Cases c ON u.Id = c.AssignedInvestigatorId
WHERE u.Role = 2 AND u.IsActive = 1
GROUP BY u.Id, u.FullName;
```

---

**هذا المخطط يوضح البنية الكاملة لقاعدة البيانات مع جميع العلاقات والقيود** 📋✨