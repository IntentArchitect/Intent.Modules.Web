## ➕ ADD ENTITY SCREEN RULES (Angular Material + Best Practice)

### Form generation rules
- Build an Angular Material form **based on the entity model defined in the TS file**.
- Bind every input to a property on `model`.
- Do not add properties that do not exist.
- Do not rename or remove properties.

### Form controls
- For each property:
  - Strings → `<input matInput>`
  - Booleans → `<mat-slide-toggle>`
  - Enums → `<mat-select>`
  - Lookups → `<mat-select>` with service-loaded options if such services exist
  - Arrays → repeatable Material blocks

### Validation
- Use template-driven Angular (`ngForm`).
- Required fields must have:
  - `required`
  - `name="xxx"`
  - `#xxxCtrl="ngModel"`
  - `<mat-error>` showing validation messages when invalid
- The Save button must be disabled if:
  - the form is invalid, or
  - `isLoading` is true.

### Save behavior
- (IMPORTANT) The **Save** button must call a `save()` method.
- The `save()` method must:
  - perform validation (via `form.invalid`)
  - call the existing service method *without modifying it*
  - on success, navigate using an existing navigation method (e.g. `navigateToEntitySearch()`)

### Forbidden actions
- DO NOT modify the existing backend-calling method (e.g. `createEntity()`).
- DO NOT change the shape of the payload.
- DO NOT invent lookup services.
- DO NOT add logic that rewrites existing TS functionality.

### Child collections
- Render them in repeatable Material UI blocks.
- Include a delete button **only if the TS file already contains a method like `removeX()`**.
- Include an add button **only if `addX()` exists**.
