# Entity Relationship Diagram (ERD)

## الكيانات والعلاقات

### 1. ApplicationUser (المستخدم)
```
ApplicationUser {
  Id (PK)
  UserName
  Email
  FullName
  Role
  CreatedAt
  IsActive
}
```

### 2. Report (البلاغ)
```
Report {
  Id (PK)
  ReporterName
  ReporterIdNumber
  ReportType
  ReportDate
  Description
  Location
  Attachments
  Status
  CreatedAt
  UpdatedAt
}
```

### 3. Case (القضية)
```
Case {
  Id (PK)
  CaseNumber
  ReportId (FK)
  InvestigatorId (FK)
  Status
  Notes
  CreatedAt
  UpdatedAt
  ClosedAt
}
```

### 4. Suspect (المتهم)
```
Suspect {
  Id (PK)
  Name
  IdNumber
  DateOfBirth
  Address
  Phone
  Nationality
  Status
  CreatedAt
  UpdatedAt
}
```

### 5. Investigator (المحقق)
```
Investigator {
  Id (PK)
  Name
  Rank
  Department
  BadgeNumber
  Email
  Phone
  IsActive
  CreatedAt
  UserId (FK)
}
```

### 6. Document (المستند)
```
Document {
  Id (PK)
  FileName
  FilePath
  FileType
  FileSize
  Description
  CaseId (FK)
  ReportId (FK)
  UploadedAt
  UploadedBy
}
```

## العلاقات

### One-to-Many Relationships:
1. **Report → Case**: بلاغ واحد يمكن أن ينتج عنه عدة قضايا
2. **Investigator → Case**: محقق واحد يمكن أن يعمل على عدة قضايا
3. **Report → Document**: بلاغ واحد يمكن أن يحتوي على عدة مستندات
4. **Case → Document**: قضية واحدة يمكن أن تحتوي على عدة مستندات
5. **ApplicationUser → Investigator**: مستخدم واحد يمكن أن يكون محقق واحد

### Many-to-Many Relationships:
1. **Report ↔ Suspect**: بلاغ واحد يمكن أن يتعلق بعدة متهمين، ومتهم واحد يمكن أن يكون في عدة بلاغات
2. **Case ↔ Suspect**: قضية واحدة يمكن أن تتعلق بعدة متهمين، ومتهم واحد يمكن أن يكون في عدة قضايا

## المفاتيح الأجنبية (Foreign Keys)

- `Case.ReportId` → `Report.Id`
- `Case.InvestigatorId` → `Investigator.Id`
- `Investigator.UserId` → `ApplicationUser.Id`
- `Document.CaseId` → `Case.Id`
- `Document.ReportId` → `Report.Id`

## جداول الربط (Junction Tables)

### ReportSuspect
```
ReportSuspect {
  Id (PK)
  ReportId (FK)
  SuspectId (FK)
  AssignedAt
  Notes
}
```

### CaseSuspect
```
CaseSuspect {
  Id (PK)
  CaseId (FK)
  SuspectId (FK)
  AssignedAt
  Notes
}
```

## الحالات (Status Values)

### Report Status:
- Pending (معلق)
- UnderInvestigation (قيد التحقيق)
- Closed (مغلق)
- Transferred (محول)

### Case Status:
- Open (مفتوح)
- UnderInvestigation (قيد التحقيق)
- Closed (مغلق)
- Transferred (محول)

### Suspect Status:
- Active (نشط)
- Arrested (مقبوض عليه)
- Released (مطلق سراحه)
- Convicted (مدان)

## القيود (Constraints)

1. **NOT NULL Constraints**:
   - جميع الحقول المطلوبة (Required)
   - التواريخ الأساسية
   - المعرفات

2. **UNIQUE Constraints**:
   - CaseNumber (رقم القضية)
   - ReporterIdNumber (رقم هوية المبلغ)
   - SuspectIdNumber (رقم هوية المتهم)
   - BadgeNumber (رقم الشارة)

3. **CHECK Constraints**:
   - Status values validation
   - Date validations
   - Email format validation

## الفهارس (Indexes)

### Primary Indexes:
- جميع المفاتيح الأساسية

### Secondary Indexes:
- ReportDate (للبحث السريع)
- Status (للفلترة)
- ReportType (للفلترة)
- CreatedAt (للفلترة الزمنية)