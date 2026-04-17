---
name: angular-page-editing-entity
description: Creates Angular edit/update entity pages using Angular Material and template-driven forms, strictly preserving existing TypeScript service and payload behavior while wiring a valid Save flow and model-bound UI.
paths:
  - "**/*.component.ts"
  - "**/*.component.html"
---

## MANDATORY: Read Samples Before Implementation

**STOP** - You MUST read ALL sample files in the SAME folder as this SKILL.md before writing ANY code:

1. `edit-entity-sample.ts`
2. `edit-entity-sample.html`
3. `edit-entity-sample.scss`

Then read target component `.ts` file and related project files (models, enums, lookups, services, styles).

**If any sample file cannot be accessed**: Stop immediately, confirm SKILL.md folder location, retry from that location. If still inaccessible, report which file and ask user. Do NOT proceed with partial implementation or approximation.

---

## Preserve Existing Implementation

**Use for**: Edit/Update entity screens in Angular with Angular Material  
**Do NOT use for**: Search/list pages, detail/view forms, or non-Angular projects  
**Source of truth**: Existing component TS file defines service calls, navigation, model structure, data loading

### You MUST NOT:
- Modify existing backend methods (e.g., `updateEntity()`)
- Change payload shape sent to backend
- Add, rename, or remove model properties
- Invent fields or lookup services
- Rewrite existing TS functionality
- Add logic that calls services/router directly (use existing methods only)

---

## 1. Data Loading & Form

**Load data**: In `ngOnInit`, load entity via existing service. Do NOT modify service method or data structure.

**Build form**: From existing model in component TS. Bind to `model` using `[(ngModel)]="model.propertyName"`. Prepopulate all values. Use only existing properties—no additions, renames, or removals.

**Nullable objects** (e.g., `loyalty: X | null`):
- Toggle/checkbox controls section (e.g., "Has Loyalty")
- Nested fields render only when toggle ON
- Maintain exact TS logic for showing/hiding
- Toggle OFF → null; toggle ON → initialize if null
- Do NOT render as required/visible unless: explicit required validation, payload always sends it, or required validators on nested fields with always non-null object

---

## 2. Map Properties to Controls

| Property Type | Control |
|---------------|---------|
| String | `<input matInput>` or `<textarea matInput>` |
| Boolean | `<mat-slide-toggle>` or `<mat-checkbox>` |
| Enum | `<mat-select>` (see Enum Rules) |
| Lookup | `<mat-select>` from **real** services only |
| Array | Repeatable Material blocks |

**Enum Rules (NO GUESSING)**:
1. **Locate** (MANDATORY): Use import path in component or code search `export enum <EnumName>`. **If not found, STOP**—do NOT reference `<EnumName>.<Member>`.
2. **Read & extract** (MANDATORY): Read enum file, extract exact member names (case-sensitive), determine string/number-valued.
3. **Use ONLY verified members** (NO INVENTING): Do NOT invent members, "correct" spelling (`Deliver` → `Delivery`), or copy from samples unless they match.
4. **Implement**: Expose to template (`AddressType = AddressType;`), use in TS defaults (`AddressType.Delivery`), use in HTML (`<mat-option [value]="AddressType.Delivery">`).
5. **Validate pre-output** (REQUIRED): Verify every `<EnumName>.<Member>` in `.html`/`.ts` exists in enum definition. Fix mismatches before output.

---

## 3. Template-Driven Forms & Validation

Use `<form #form="ngForm">`.

**Required fields need ALL**:
- `required`, `name="xxx"` (unique), `#xxxCtrl="ngModel"`, `<mat-error>` (show when invalid & touched)

**Form state**:
- Save button disabled when `form.invalid || isLoading`
- `save(form)` checks `form.invalid`, prevents backend call when invalid

---

## 4. Save Flow

Save button calls `save()` method that:
1. Validates via `form.invalid`
2. Calls existing service method (e.g., `updateEntity()`) without modification
3. On success, navigates via existing navigation method (e.g., `navigateToEntitySearch()`)

**Forbidden**: Call service from template, modify service methods, change payload, add new `router.navigate(...)`.

---

## 5. Child Collections

Render in repeatable Material blocks (e.g., `addresses`, `phones`).

**Buttons**: Add only if `addX()` exists in TS (e.g., `addAddress()`). Remove only if `removeX()` exists in TS (e.g., `removeAddress(i)`). Do NOT invent.

---

## 6. Styling

- **Global utilities first**: `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`
- **Component SCSS**: Add when sample uses component styling and global doesn't provide it
- **Match sample layout visually**
- **NEVER modify** `styles.scss`/`theme.scss` (add only if reusable across pages)
- **Component SCSS required**: Edit-entity sample needs component SCSS for layout. Copy/adapt unless exact styles exist globally.

---

## Definition of Done

**Template compilation**:
- [ ] All bound properties exist in TS (`model.*`, `isLoading`, `serviceErrors.*`, lookups)
- [ ] All methods exist (`save(form)`, `addX()`, `removeX()`, navigation)
- [ ] Directives valid (`*ngIf`, `*ngFor`, `(click)`)

**Data loading**:
- [ ] Data loaded in `ngOnInit` via existing service
- [ ] Form prepopulated using `[(ngModel)]="model.propertyName"`
- [ ] No modifications to data-loading service

**Form fidelity**:
- [ ] Controls bind only to existing model properties
- [ ] No properties renamed/removed/retyped
- [ ] Controls follow section 2 mapping

**Template-driven forms**:
- [ ] Uses `<form #form="ngForm">`
- [ ] Every `[(ngModel)]` has unique `name`
- [ ] Required fields: `required`, `name`, `#ctrl="ngModel"`, `<mat-error>`
- [ ] Save disabled when `isLoading || form.invalid`
- [ ] `save(form)` prevents call when invalid

**Save flow**:
- [ ] Save calls `save(form)`, not service directly
- [ ] Backend methods (e.g., `updateEntity()`) not modified
- [ ] Payload matches existing TS exactly
- [ ] Navigation uses existing method

**Nullable objects & conditional sections**:
- [ ] Toggle/checkbox controls visibility
- [ ] Toggle OFF → null; ON → initialize
- [ ] Nested fields render only when enabled
- [ ] No unsafe `model.obj!.field`
- [ ] Conditional sections maintain exact TS logic

**Child collections**:
- [ ] Add buttons only if `addX()` in TS
- [ ] Remove buttons only if `removeX()` in TS

**Enums**:
- [ ] Options match real enum members
- [ ] No names from sample without verification
- [ ] Defaults compile against actual type
- [ ] Every `<EnumName>.<Member>` verified before output

**Styling**:
- [ ] No modifications to `styles.scss`/`theme.scss`
- [ ] Component SCSS minimal, prefers global utilities
- [ ] Component SCSS adapted from sample when needed



---

## 1. Data Loading & Form

**Load data**: In `ngOnInit`, load entity via existing service. Do NOT modify service method or data structure.

**Build form**: From existing model in component TS. Bind to `model` using `[(ngModel)]="model.propertyName"`. Prepopulate all values. Use only existing properties—no additions, renames, or removals.

**Nullable objects** (e.g., `loyalty: X | null`):
- Toggle/checkbox controls section (e.g., "Has Loyalty")
- Nested fields render only when toggle ON
- Maintain exact TS logic for showing/hiding
- Toggle OFF → null; toggle ON → initialize if null
- Do NOT render as required/visible unless: explicit required validation, payload always sends it, or required validators on nested fields with always non-null object

---

## 2. Map Property Types to Material Controls

| Property Type | Control |
|---------------|---------|
| String | `<input matInput>` or `<textarea matInput>` when appropriate |
| Boolean | `<mat-slide-toggle>` or `<mat-checkbox>` |
| Enum | `<mat-select>` with explicit enum values (see Enum Rules below) |
| Lookup | `<mat-select>` populated ONLY from options loaded by existing services (no invented services) |
| Array | Repeatable Material blocks bound to each item |

### CRITICAL: Enum Handling (NO GUESSING)

For each enum field in the model, follow this mandatory process:

1. **Locate the enum definition** (MANDATORY):
   - Use direct import path already present in component (e.g., `import { AddressType } from '...'`)
   - Otherwise use code search for `export enum <EnumName>` or `enum <EnumName>`
   - **If enum cannot be located, STOP**—do NOT reference `<EnumName>.<Member>` in templates or TS

2. **Read and extract** (MANDATORY):
   - Read the enum file and extract exact member identifiers (case-sensitive)
   - Determine if enum is string-valued or number-valued
   - Record the allowed set of members

3. **Use ONLY verified members** (NO INVENTING):
   - Do NOT invent members
   - Do NOT "correct" spelling (e.g., `Deliver` → `Delivery`) without enum file proving it
   - Do NOT copy members from skill examples unless they match extracted set

4. **Template + TS implementation**:
   - Expose enum to template after verification (e.g., `AddressType = AddressType;`)
   - Default initialization uses verified member (e.g., `AddressType.Delivery`)
   - Template options use verified members (e.g., `<mat-option [value]="AddressType.Delivery">`)

5. **Pre-output validation** (REQUIRED):
   - Verify every `<EnumName>.<Member>` referenced in `.html` and `.ts` exists in the enum definition
   - Fix any mismatches before generating output

---

## 3. Template-Driven Forms & Validation

Use template-driven Angular forms (`<form #form="ngForm">`).

### Required field implementation (ALL of the following):
- `required` attribute
- `name="xxx"` (unique, including inside `*ngFor` blocks)
- `#xxxCtrl="ngModel"` template variable
- `<mat-error>` with validation message shown when invalid and touched

### Form validation state:
- Save button disabled when `form.invalid` OR `isLoading` is true
- `save(form)` method must check `form.invalid` and prevent backend call when invalid

---

## 4. Save Flow Preservation

The Save button MUST call a `save()` method that:
1. Validates via `form.invalid`
2. Calls the existing service method (e.g., `updateEntity()`) without modification
3. On success, navigates using existing navigation method (e.g., `navigateToEntitySearch()`)

**Do NOT**:
- Call service methods directly from template
- Modify existing service-calling methods
- Change the request payload structure
- Introduce new `router.navigate(...)` logic

---

## 5. Child Collections

Render child collections (e.g., `addresses`, `phones`) in repeatable Material UI blocks.

### Action button rules:
- **Add button**: Only render if TS file contains matching `addX()` method (e.g., `addAddress()`)
- **Delete/Remove button**: Only render if TS file contains matching `removeX()` method (e.g., `removeAddress(i)`)
- Do NOT invent these methods—they must already exist

---

## 6. Styling Requirements

- **Use global utilities first**: `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.
- **Add component SCSS** when sample uses component-scoped styling for layout and global styles don't provide it
- **The generated page must visually match the sample layout**
- **Add new utilities to `styles.scss`** only when reusable across multiple pages
- **NEVER modify** existing styles in `styles.scss` or `theme.scss`—only add new ones
- **Component SCSS required**: The edit-entity sample relies on component SCSS for layout. Copy/adapt sample SCSS unless those exact styles exist globally. Do not leave component SCSS empty if template uses classes that aren't confirmed global.

---

## 7. Mandatory: Verify DTO/Wrapper Shapes Before Template Binding
When binding in HTML to a property that is not directly declared on the component (e.g., `customersModels?.X`, `response?.X`, `paged?.X`, `result?.X`), you MUST verify the shape of the type:

- If the property is a generic wrapper (e.g., `PagedResult<T>`, `ListResult<T>`, `ApiResponse<T>`), you MUST:
	- Navigate to its definition (via import path) and read the file.
	- Use the exact collection property name from the type (e.g., data vs items).

- For any `*ngFor`, you MUST confirm the iterated expression resolves to an array type in the codebase.
	- Forbidden: assuming common names like items, results, value, content.
	- Required: use only verified members from the actual interface/class.

- Output requirement:
	- If the wrapper type cannot be located/read, STOP and ask the user which property contains the collection.

No `*ngFor` over ?.items unless the type definition explicitly contains items

---

## Definition of Done

### Template compilation:
- [ ] All bound properties exist in TS (e.g., `model.*`, `isLoading`, `serviceErrors.*`, lookup arrays)
- [ ] All called methods exist in TS (e.g., `save(form)`, `addX()`, `removeX()`, navigation methods)
- [ ] All structural directives are syntactically valid (`*ngIf`, `*ngFor`, `(click)`)

### Data loading:
- [ ] Data loaded in `ngOnInit` using existing service
- [ ] Form values prepopulated using `[(ngModel)]="model.propertyName"`
- [ ] No modifications to data-loading service method

### Form fidelity:
- [ ] All inputs/selects/toggles bind only to existing model properties (no invented fields)
- [ ] No model properties renamed, removed, or retyped
- [ ] All controls follow Material mapping rules from section 2

### Template-driven forms:
- [ ] Form uses `<form #form="ngForm">`
- [ ] Every `[(ngModel)]` has unique `name="..."` attribute
- [ ] Required fields include: `required`, `name`, `#ctrl="ngModel"`, `<mat-error>`
- [ ] Save button disabled when `isLoading || form.invalid`
- [ ] `save(form)` prevents backend call when `form.invalid`

### Save flow:
- [ ] Save button calls `save(form)`, not service method directly
- [ ] Existing backend methods (e.g., `updateEntity()`) not modified
- [ ] Request payload matches existing TS mapping exactly
- [ ] Post-save navigation uses existing navigation method

### Nullable objects & conditional sections:
- [ ] Optional by default via toggle/checkbox
- [ ] Toggle OFF sets to null; toggle ON initializes if null
- [ ] Nested fields render only when enabled
- [ ] No unsafe `model.obj!.field` usage when it can be null
- [ ] Conditional sections maintain exact logic from TS file

### Child collections:
- [ ] Add buttons exist only if `addX()` method exists in TS
- [ ] Remove buttons exist only if `removeX()` method exists in TS
- [ ] No validation/name collisions when items added/removed

### Enums:
- [ ] Every enum option matches real enum member from codebase
- [ ] No enum names copied from sample without verification
- [ ] Enum default values compile against actual enum type
- [ ] Every `<EnumName>.<Member>` reference verified before output

### Styling:
- [ ] Existing `styles.scss` and `theme.scss` not modified (only additions if needed)
- [ ] Component SCSS minimal, preferring global utilities
- [ ] All CSS classes used are defined (globally or component-scoped)
- [ ] Component SCSS adapted from sample when sample uses component styling