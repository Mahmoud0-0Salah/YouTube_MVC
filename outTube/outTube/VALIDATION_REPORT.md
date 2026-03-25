# outTube Validation Guide 🛡️

This guide provides an overview of the custom validation logic implemented to ensure data integrity, security, and a clean community environment in the outTube project.

---

## 🏗️ Custom Validation Attributes
*Location: `Validation/Attributes/`*

These are reusable data annotation attributes that can be applied directly to Model/ViewModel properties.

### 1. `AllowedExtensionsAttribute`
- **Purpose**: Restricts file uploads to specific extensions (e.g., `.mp4`, `.jpg`).
- **Why**: Prevents unsupported or dangerous file types from being uploaded.
- **Usage**:
  ```csharp
  [AllowedExtensions(new[] { ".mp4", ".mov" })]
  public IFormFile VideoFile { get; set; }
  ```

### 2. `FutureDateAttribute`
- **Purpose**: Prevents selecting a date that hasn't happened yet.
- **Why**: Essential for fields like "Date of Birth" which must be in the past.
- **Usage**:
  ```csharp
  [FutureDate]
  public DateTime BirthDate { get; set; }
  ```

### 3. `MaxFileSizeAttribute`
- **Purpose**: Limits file size (in MB).
- **Why**: Avoids server storage issues and excessive bandwidth usage.
- **Usage**:
  ```csharp
  [MaxFileSize(500)] // Limit to 500 MB
  public IFormFile File { get; set; }
  ```

### 4. `MinimumAgeAttribute`
- **Purpose**: Enforces an age requirement based on a birth date.
- **Why**: Used for platform compliance (e.g., requiring users to be 13+).
- **Usage**:
  ```csharp
  [MinimumAge(13)]
  public DateTime DateOfBirth { get; set; }
  ```

### 5. `NoProfanityAttribute`
- **Purpose**: Filters out a pre-defined list of inappropriate words.
- **Why**: Maintains a respectful community and filters toxic content.
- **Usage**:
  ```csharp
  [NoProfanity]
  public string CommentText { get; set; }
  ```

### 6. `NotSelfReferenceAttribute`
- **Purpose**: Cross-references two properties to ensure they aren't identical.
- **Why**: Prevents "self-actions" like subscribing to yourself or reporting yourself.
- **Usage**:
  ```csharp
  public string CurrentUserId { get; set; }
  [NotSelfReference("CurrentUserId")]
  public string TargetUserId { get; set; }
  ```

### 7. `SafeUrlAttribute`
- **Purpose**: Validates absolute URLs and restricts allowed protocols (default: HTTPS).
- **Why**: Prevents malicious links and ensures secure data transit.
- **Usage**:
  ```csharp
  [SafeUrl(new[] { "https" })]
  public string Website { get; set; }
  ```

---

## ⚡ FluentValidation Validators
*Location: `Validation/Validators/`*

These classes contain complex, multi-field validation logic using the FluentValidation library.

### 🎥 `VideoCreateValidator`
- **Title**: 3-255 characters; no HTML; no profanity.
- **Description**: Max 255 characters; no HTML.
- **Video File**: Required; specific extensions; max 500 MB.
- **Thumbnail**: Specific extensions; max 5 MB.

### 👤 `UserRegisterValidator`
- **Name**: Required; 2-255 chars; letters only.
- **Email**: Required; valid format.
- **Password**: 8+ chars; requires Uppercase, Lowercase, Digit, and Special Character.
- **Age**: Must be at least 13 years old.
- **Profile Image**: Must be a valid HTTPS URL.

### 💬 `CommentCreateValidator`
- **Content**: Required; 1-255 characters; no HTML; no profanity; no whitespace-only comments.

### 🚩 `ReportCreateValidator`
- **Reason**: Required; 5-255 characters.
- **Description**: Max 255 characters.

### 🔔 `SubscribeValidator`
- **Self-Check**: Dynamically retrieves the current user session via `IHttpContextAccessor` to block self-subscriptions.

---

## 📱 ViewModels & Applied Validations
This section shows how the above attributes and validators are applied to the project's ViewModels.

### 👤 User Management
- **`UserRegisterViewModel`**: 
  - Enforces **MinimumAge(13)** and **FutureDate** on `DateOfBirth`.
  - Uses **SafeUrl** for `ImageUrl`.
  - Strict regex for names and passwords.
- **`UserProfileEditViewModel`**:
  - Combines **AllowedExtensions** and **MaxFileSize** for profile image uploads.
  - Re-uses **MinimumAge** and **SafeUrl** logic.

### 🎥 Video Operations
- **`VideoCreateViewModel`**:
  - Implements **NoProfanity** on `Title` and `Description`.
  - Restricts `VideoFile` to 500 MB and specific formats via **MaxFileSize** and **AllowedExtensions**.
  - Limits `ThumbnailFile` to 5 MB.
- **`VideoEditViewModel`**:
  - Similar to creation, but allows optional file updates while maintaining strict validation.

### 📜 Administrative & Social
- **`LoginViewModel` / `UserLoginViewModel`**:
  - Validates email format and password presence.
- **`ProfileViewModel`**:
  - Uses **Required** attributes for profile data and **SafeUrl** for existing images.
- **`CommentCreateViewModel` / `CommentEditViewModel`**:
  - Uses **NoProfanity** and length constraints to keep comments clean.
- **`ReportCreateViewModel` / `ReportReviewViewModel`**:
  - Handles report submission and administrative review with status constraints.
- **`BanUserViewModel`**:
  - Ensures a valid `UserId` is provided for administrative actions.
- **`SubscribeViewModel`**:
  - Validates the target `UserId` and prevents self-subscription through the associated validator.
