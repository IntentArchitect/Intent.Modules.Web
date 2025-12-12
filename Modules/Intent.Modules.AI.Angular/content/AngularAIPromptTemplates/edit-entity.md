
### Source of truth
- Use the entity model loaded into the component (e.g., `model` with data from `loadEntityById()`).
- Do not modify the shape of the model.
- Do not invent new fields.

### Form controls
- Same field mappings as the Add screen.
- Prepopulate values using `[(ngModel)]="model.propertyName"`.

### Validation
- Form must use `<form #form="ngForm" ...>`.
- Required fields should have:
  - `required`
  - validation errors via `<mat-error>`
  - Save disabled when invalid

### Save behavior
- The Save button must call `save()`.
- `save()` must:
  - validate the form
  - call the existing service method (e.g. `updateEntity()`)
  - **NOT** modify that method
  - on success, navigate using an existing method such as `navigateToEntitySearch()`

### Conditional sections
- If the TS model uses boolean toggles (like `hasLoyalty`):
  - Only render conditional blocks when flagged.
  - Maintain the exact logic from the TS file for showing/hiding these blocks.

### Child collections
- When arrays exist (`addresses`, `phones`, etc.):
  - Render list items in Material blocks.
  - Include add/remove buttons **only if corresponding TS methods exist**:
    - `addAddress()` → show Add button
    - `removeAddress(i)` → show Remove button

### Forbidden actions
- DO NOT modify:
  - existing service methods (`updateEntity`, etc.)
  - DTO structure
  - models or nested models
