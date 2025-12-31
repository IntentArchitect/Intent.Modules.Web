
### Source of truth
- In the ngOnInit load the data the component needs using the provided service and data.
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

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- Component `.scss` files should remain minimal - only add truly component-specific styles
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed