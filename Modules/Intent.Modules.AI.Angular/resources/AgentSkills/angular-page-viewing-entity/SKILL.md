---
name: angular-page-viewing-entity
description: Creates Angular view/detail entity pages for displaying read-only entity data using Angular Material components, strictly preserving existing TypeScript service behavior while presenting data in a clean, organized layout.
paths:
	- "**/*.component.ts"
	- "**/*.component.html"
---

## Reference Examples

Before generating any output, read the following files in the same folder as this skill:

1. **`./view-entity-sample.ts`** - MUST be read and used as the TypeScript structure template (imports, component metadata, data loading, navigation methods, and helper properties) and adapt names and types for the target entity.
2. **`./view-entity-sample.html`** - MUST be read and used as the HTML layout template. Copy its layout and Angular Material read-only field presentation, then adapt fields, bindings, and labels for the target entity.
3. **`./view-entity-sample.scss`** - MUST be read whenever implementing the component, even if you expect minimal styling.

---

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.grid`, `.grid-item`, `.grid-item-wide`, `.section-title`, `.address-block`, `.pa-4`, `.mb-4`, `.ux-gradient-primary`, etc.)
- SCSS decision rule:
  - Use global utilities where possible.
  - If the sample uses component SCSS for structural layout (grid columns, responsive behavior, address blocks) and global styles don't already guarantee it, copy/adapt the sample SCSS into the component SCSS.
  - Only add to styles.scss when the style is reusable across multiple pages; otherwise keep it component-scope
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed
- Copy/adapt sample SCSS into the component SCSS unless you can prove the same classes already exist globally.
- The generated page must visually match the sample layout.
- SCSS Parity Rule (Required): The view-entity sample relies on component-scoped SCSS for layout. Therefore, when generating a view page, the agent must copy/adapt the sample SCSS into the component SCSS unless those exact styles already exist in styles.scss. The agent must not leave component SCSS empty if the template uses classes that aren't confirmed global.

### 1. Source of truth and data loading (read-only)
- In the ngOnInit, load the entity data using the provided service (e.g., `getCustomerById(id)`).
- Extract the entity ID from route parameters using `ActivatedRoute`.
- Display entity data in read-only format based on the model returned from the service.
- Do not modify the shape of the model.
- Do not add properties that do not exist.
- Do not rename or remove properties.
- Do not invent new fields.

### 2. Map property types to correct read-only Angular Material controls
- For each property, use **read-only** presentation:
	- Strings -> `<input matInput [value]="model.propertyName" readonly />`
	- Booleans -> `<mat-slide-toggle [checked]="model.propertyName" disabled>`
	- Enums -> `<input matInput [value]="enumDisplayValue" readonly />` (convert enum to display text)
    - When rendering enum fields (e.g., AddressType, Status, etc.):
      - The enum must be treated as the source of truth from the actual imported enum definition in the target project, not from the skill sample.
      - Convert enum values to human-readable text (e.g., `AddressType.Deliver` -> `'Delivery'`)
      - Display in readonly input fields
	- Lookups -> `<input matInput [value]="model.categoryName" readonly />` (use the display name property)
	- Arrays -> Display as repeatable read-only Material blocks (see section 6)
- All form fields must use `readonly` attribute or `disabled` for toggles.
- Use `[value]` binding for inputs, NOT `[(ngModel)]` (no two-way binding needed for read-only views).
- Add decorative disabled icon buttons with `matPrefix` for visual appeal (e.g., `<button mat-icon-button matPrefix disabled><mat-icon>person</mat-icon></button>`).

### 3. No forms or validation required
- This is a **read-only view**, NOT a form:
  - DO NOT use `<form>` tags
  - DO NOT use `ngForm` or `[(ngModel)]`
  - DO NOT add validation attributes (`required`, etc.)
  - DO NOT add `name` attributes
  - DO NOT add `<mat-error>` elements
- All fields are display-only using `readonly` or `disabled` attributes.

### 4. No save behavior (navigation only)
- There is NO save button or save functionality.
- Provide a "Back" or "Back to List" button that:
  - Calls an existing navigation method (e.g., `navigateToCustomerSearch()`)
  - Uses `mat-raised-button` with `color="accent"` and back arrow icon
  - Example: `<button mat-raised-button color="accent" (click)="navigateToCustomerSearch()">Back to list</button>`
- DO NOT modify existing navigation methods.

### 5. Conditional sections (read-only display)
- If the model has nullable objects (like `loyalty: LoyaltyDto | null`):
  - Use a computed property (e.g., `get hasLoyalty(): boolean { return !!this.model?.loyalty; }`)
  - Display a disabled toggle showing the state: `<mat-slide-toggle [checked]="hasLoyalty" disabled>Has Loyalty</mat-slide-toggle>`
  - Use `*ngIf` to conditionally render nested fields when present
  - Display nested fields in readonly format when present
- Do NOT add logic to toggle or modify these values (read-only view).
- Maintain the exact conditional rendering logic from the TypeScript file.

### 6. Child collections (read-only display)
- When arrays exist (`addresses`, `phones`, etc.):
  - Render each item in a styled read-only Material block (e.g., `.address-block`)
  - Use `*ngFor` to iterate over items
  - Display all fields within each item as readonly inputs
  - Add visual styling with background colors and borders (see sample SCSS)
- Do NOT add "Add" or "Remove" buttons (read-only view).
- Do NOT make collections editable.

### 7. Loading and error states
- Show a loading spinner while data is being fetched:
  - Use `*ngIf` with else clause pattern
  - Create a `<ng-template #loadingTpl>` with `<mat-progress-spinner>`
  - Set `isLoading = true` before service call, `false` in `finalize()` operator
- Display service errors prominently:
  - Use `.error-container` class for error messages
  - Example: `<div *ngIf="serviceErrors.loadError" class="error-container">...</div>`
- Handle missing entity ID gracefully with error throw in ngOnInit.

### 8. Forbidden actions
- DO NOT modify existing backend-calling methods (e.g., `getCustomerById()`).
- DO NOT add edit, save, update, or delete functionality (this is view-only).
- DO NOT invent lookup services.
- DO NOT add logic that rewrites existing TypeScript functionality.
- DO NOT add two-way data binding (`[(ngModel)]`).
- DO NOT add forms or validation.

### 9. General constraints
- Use this skill for View/Detail/Display entity screens in Angular.
- Do not use this skill for edit/update pages, search/list pages, or non-Angular projects.
- Treat the existing component TypeScript file as the source of truth for service calls and navigation behavior.
- All UI elements should be read-only (no editing capabilities).

### 10. Enum fields Required Implementation Steps
For each enum used by the target entity model:
- Locate the enum definition:
  - Prefer direct import path already present in the component (import { AddressType } from '...')
  - Otherwise use code_search for export enum AddressType (or the enum name)
- Read the enum file and extract exact member names and values.
- Convert enum values to human-readable display text:
  - Expose the enum to the template (e.g., `AddressType = AddressType;`)
  - Use conditional expressions to display text (e.g., ternary operator for simple cases)
  - Or create a helper method: `getAddressTypeDisplay(type: AddressType): string`

## Completion Checklist

- [] The HTML template compiles and every referenced symbol exists in the component TS:
  - [] All bound properties exist (e.g., model.*, isLoading, serviceErrors.*, enum types).
  - [] All called methods exist (e.g., loadEntityById(), navigation methods, computed properties like hasLoyalty).
  - [] All structural directives and bindings are syntactically valid (*ngIf, *ngFor, [value], [checked]).
- [] The view is strictly data-driven (read-only):
  - [] All displayed fields come from properties that exist on the loaded model (no invented fields).
  - [] No model properties were renamed, removed, or retyped to satisfy the UI.
  - [] Data is loaded in ngOnInit using the existing service and route parameters.
  - [] Entity ID is extracted from route params with proper null checking.
- [] All controls are read-only and follow Angular Material patterns:
  - [] Strings use `<input matInput [value]="..." readonly />`.
  - [] Booleans use `<mat-slide-toggle [checked]="..." disabled>`.
  - [] Enums display as human-readable text in readonly inputs.
  - [] Lookups display the name/label property in readonly inputs.
  - [] Arrays render as repeatable read-only blocks with proper styling.
  - [] Decorative disabled icon buttons with matPrefix are used for visual appeal.
- [] No forms or editing functionality:
  - [] NO `<form>` tag is used.
  - [] NO `[(ngModel)]` two-way binding is used (only [value] or [checked]).
  - [] NO validation attributes (required, name, etc.) are present.
  - [] NO `<mat-error>` elements exist.
  - [] NO save, update, or delete buttons exist.
- [] Navigation is correctly implemented:
  - [] A "Back" or "Back to List" button exists.
  - [] Button calls an existing navigation method (e.g., navigateToCustomerSearch()).
  - [] No navigation methods were modified or invented.
- [] Conditional sections are displayed correctly (read-only):
  - [] Nullable object sections use computed properties (e.g., get hasLoyalty(): boolean).
  - [] Disabled toggles show the state of nullable sections.
  - [] Nested fields render only when the section exists using *ngIf.
  - [] No toggle functionality exists (disabled toggles are display-only).
- [] Child collections are displayed correctly:
  - [] Collections use *ngFor to iterate over items.
  - [] Each item renders in a styled block (e.g., .address-block).
  - [] All fields within items are readonly.
  - [] NO "Add" or "Remove" buttons exist.
- [] Loading and error states are handled:
  - [] Loading spinner displays while data is being fetched.
  - [] *ngIf pattern with else clause is used for loading state.
  - [] Service errors display in .error-container elements.
  - [] isLoading flag is properly managed with finalize() operator.
  - [] Missing entity ID throws an error in ngOnInit.
- [] Styling rules are respected:
  - [] Existing styles.scss and theme.scss were not modified (only additive changes allowed if necessary).
  - [] Component SCSS includes layout-specific styles (grid, address blocks, sections).
  - [] Global utility classes are used where appropriate (.grid, .grid-item, .section-title, etc.).
  - [] If the sample template uses component SCSS for layout, the target component SCSS includes an adapted version.
- [] Enum handling is correct:
  - [] Every enum used is imported and exposed to the template.
  - [] Enum values are converted to human-readable display text.
  - [] No enum member names were copied from the sample without verification.
  - [] Enum display logic matches the actual enum definition from the codebase.
- [] All bindings between HTML and TS are correct:
  - [] All referenced properties and methods exist in the component TS.
  - [] Code compiles with all necessary imports.
  - [] No invented properties, methods, or services.
