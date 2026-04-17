---
name: angular-page-adding-entity
description: Creates Angular add/create entity pages using Angular Material and template-driven forms, strictly preserving existing TypeScript service and payload behavior while wiring a valid Save flow and model-bound UI.
paths:
  - "**/*.component.ts"
  - "**/*.component.html"
---

## MANDATORY: Read Samples Before Implementation

**STOP** - You MUST read ALL sample files in the SAME folder as this SKILL.md before writing ANY code:

1. `add-entity-sample.ts`
2. `add-entity-sample.html`
3. `add-entity-sample.scss`

Then read target component `.ts` file and related project files (models, enums, lookups, services, styles).

**If any sample file cannot be accessed**: Stop immediately, confirm SKILL.md folder location, retry from that location. If still inaccessible, report which file and ask user. Do NOT proceed with partial implementation or approximation.

---

## Preserve Existing Implementation

**Use for**: Add/Create entity screens in Angular with Angular Material  
**Do NOT use for**: Search/list pages, detail/edit/view forms, or non-Angular projects  
**Source of truth**: Existing component TS file defines service calls, navigation, model structure

### You MUST NOT:
- Modify existing backend methods (e.g., `createEntity()`)
- Change payload shape sent to backend
- Add, rename, or remove model properties
- Invent lookup services
- Rewrite existing TS functionality
- Add logic that calls services/router directly (use existing methods only)



---

## 1. Form: Build from Existing Model Only

Bind to properties on `model` that already exist. Do NOT add, rename, or remove properties.

**Nullable objects** (e.g., `loyalty: X | null`):
- Toggle/checkbox controls section (e.g., "Has Loyalty")
- Nested fields render only when toggle ON
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

**Enum Rules**:
1. Locate definition: Use import path in component or code search `export enum AddressType`
2. Read enum file, extract exact member names/values
3. Use exact members in TS default (`AddressType.Delivery`) and HTML (`<mat-option [value]="AddressType.Delivery">`)
4. **Forbidden**: Assume member names from sample. Target project enum is source of truth.

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
2. Calls existing service method (no modification)
3. On success, navigates via existing navigation method (e.g., `navigateToEntitySearch()`)

**Forbidden**: Call service from template, modify service methods, change payload, add new `router.navigate(...)`.

---

## 5. Child Collections

Render in repeatable Material blocks.

**Buttons**: Add only if `addX()` exists in TS. Remove only if `removeX()` exists in TS. Do NOT invent.

---

## 6. Styling

- **Global utilities first**: `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`
- **Component SCSS**: Add when sample uses component styling and global doesn't provide it
- **Match sample layout visually**
- **NEVER modify** `styles.scss`/`theme.scss` (add only if reusable across pages)
- **Component SCSS required**: Add-entity sample needs component SCSS for layout. Copy/adapt unless exact styles exist globally.

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

**Template compilation**:
- [ ] All bound properties exist in TS (`model.*`, `isLoading`, `serviceErrors.*`, lookups)
- [ ] All methods exist (`save(form)`, `addX()`, `removeX()`, navigation)
- [ ] Directives valid (`*ngIf`, `*ngFor`, `(click)`)

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
- [ ] Backend methods not modified
- [ ] Payload matches existing TS exactly
- [ ] Navigation uses existing method

**Nullable objects**:
- [ ] Toggle/checkbox controls visibility
- [ ] Toggle OFF → null; ON → initialize
- [ ] Nested fields render only when enabled
- [ ] No unsafe `model.obj!.field`

**Child collections**:
- [ ] Add buttons only if `addX()` in TS
- [ ] Remove buttons only if `removeX()` in TS

**Enums**:
- [ ] Options match real enum members
- [ ] No names from sample without verification
- [ ] Defaults compile against actual type

**Styling**:
- [ ] No modifications to `styles.scss`/`theme.scss`
- [ ] Component SCSS minimal, prefers global utilities
- [ ] Component SCSS adapted from sample when needed