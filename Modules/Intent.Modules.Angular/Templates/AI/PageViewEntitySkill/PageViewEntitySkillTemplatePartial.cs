using System;
using System.Collections.Generic;
using System.Linq;
using Intent.Engine;
using Intent.Metadata.Models;
using Intent.Modules.Common;
using Intent.Modules.Common.FileBuilders.MarkdownFileBuilder;
using Intent.Modules.Common.Templates;
using Intent.RoslynWeaver.Attributes;
using Intent.Templates;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.ProjectItemTemplate.Partial", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.AI.PageViewEntitySkill
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class PageViewEntitySkillTemplate : MarkdownBaseTemplate<object>, IMarkdownFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.AI.PageViewEntitySkillTemplate";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public PageViewEntitySkillTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            WithContentHashing = true;
            MarkdownFile = new MarkdownFile($"SKILL", "md", "angular-page-viewing-entity")
                .FromMarkdown($$"""
---
name: angular-page-viewing-entity
description: Creates Angular view/detail entity pages for displaying read-only entity data using Angular Material components, strictly preserving existing TypeScript service behavior while presenting data in a clean, organized layout.
template-id: {{TemplateId}}
paths:

---

## MANDATORY: Read Samples Before Implementation

STOP - You MUST read ALL sample files in the SAME folder as this SKILL.md before writing ANY code:

1. `view-entity-sample.ts`
2. `view-entity-sample.html`
3. `view-entity-sample.scss`

Then read target component `.ts` file and related project files (models, enums, lookups, services, styles).

If any sample file cannot be accessed: Stop immediately, confirm SKILL.md folder location, retry from that location. If still inaccessible, report which file and ask user. Do NOT proceed with partial implementation or approximation.

## Preserve Existing Implementation & Read-Only Nature

Use for: View/Detail/Display entity screens in Angular with Angular Material  
Do NOT use for: Edit/update pages, search/list pages, or non-Angular projects  
Source of truth: Existing component TS file defines service calls, navigation, model structure  
READ-ONLY view: No forms, no editing, no validation, no save functionality

### You MUST NOT:
- Modify existing backend methods (e.g., `getCustomerById()`)
- Add edit, save, update, or delete functionality
- Invent lookup services
- Rewrite existing TS functionality
- Add two-way binding (`[(ngModel)]`)
- Add forms (`<form>`), validation attributes, or `<mat-error>` elements
- Add, rename, or remove model properties
- Make any UI elements editable

## 1. Data Loading (Read-Only)

Load data: In `ngOnInit`, load entity via existing service (e.g., `getCustomerById(id)`). Extract ID from route params using `ActivatedRoute`. Handle missing ID gracefully (throw error). Display in read-only format. Use only existing properties—no additions, renames, or removals.

Loading & error states:
- Loading spinner: `*ngIf` with else clause, `<ng-template #loadingTpl>` with `<mat-progress-spinner>`, `isLoading = true` before call, `false` in `finalize()`
- Errors: `.error-container` class, e.g., `<div *ngIf="serviceErrors.loadError" class="error-container">...</div>`

## 2. Map Properties to Read-Only Controls

All fields **display-only** with `readonly` or `disabled`. Use `[value]` or `[checked]` (NO `[(ngModel)]`).

| Property Type | Control |
|---------------|---------|
| String | `<input matInput [value]="model.propertyName" readonly />` |
| Boolean | `<mat-slide-toggle [checked]="model.propertyName" disabled>` |
| Enum | `<input matInput [value]="enumDisplayValue" readonly />` (convert to text—see Enum Rules) |
| Lookup | `<input matInput [value]="model.categoryName" readonly />` (use display name) |
| Array | Repeatable read-only Material blocks (see section 4) |

Visual enhancements: Add decorative disabled icon buttons with `matPrefix`, e.g., `<button mat-icon-button matPrefix disabled><mat-icon>person</mat-icon></button>`

Enum Rules:
1. Locate: Use import path in component or code search `export enum AddressType`
2. Read: Extract exact member names/values
3. Convert: Expose to template (`AddressType = AddressType;`), use conditionals or helper method (`getAddressTypeDisplay(type: AddressType): string`)
4. Forbidden: Do NOT assume member names from sample. Target project enum is source of truth.

## 3. No Forms or Validation

READ-ONLY view, NOT a form.

Do NOT include: `<form>` tags, `ngForm`, `[(ngModel)]` (use `[value]`/`[checked]` only), validation attributes (`required`, `name`), `<mat-error>`, save/update/delete buttons.

## 4. Navigation Only (No Save)

NO save button or save functionality.

Back button (required): Calls existing navigation method (e.g., `navigateToCustomerSearch()`). Use `mat-raised-button color="accent"` with back arrow icon. Example: `<button mat-raised-button color="accent" (click)="navigateToCustomerSearch()"><mat-icon>arrow_back</mat-icon>Back to List</button>`

Forbidden: Modify or invent navigation methods.

## 5. Conditional Sections & Child Collections (Read-Only)

Nullable objects (e.g., `loyalty: LoyaltyDto | null`):
- Computed property (e.g., `get hasLoyalty(): boolean { return !!this.model?.loyalty; }`)
- Disabled toggle shows state: `<mat-slide-toggle [checked]="hasLoyalty" disabled>Has Loyalty</mat-slide-toggle>`
- `*ngIf` conditionally renders nested fields when present
- Display nested fields readonly
- Do NOT add toggle logic (display-only)

Child collections (e.g., `addresses`, `phones`):
- Render each item in styled read-only block (e.g., `.address-block`)
- Use `*ngFor` to iterate
- All fields readonly inputs
- Visual styling with backgrounds/borders (see sample SCSS)
- Do NOT add "Add"/"Remove" buttons
- Do NOT make editable

## 6. Styling

- **Global utilities first**: `.grid`, `.grid-item`, `.grid-item-wide`, `.section-title`, `.address-block`, `.pa-4`, `.mb-4`, `.ux-gradient-primary`
- **Component SCSS**: Add for layout-specific styles (grid, address blocks, sections) when global doesn't provide it
- **Match sample layout visually**
- **NEVER modify** `styles.scss`/`theme.scss` (add only if reusable across pages)
- **Component SCSS required**: View-entity sample needs component SCSS for layout. Copy/adapt unless exact styles exist globally.

## Definition of Done

Template compilation:
- [ ] All bound properties exist in TS (`model.*`, `isLoading`, `serviceErrors.*`, enums)
- [ ] All methods exist (`loadEntityById()`, navigation, computed properties like `hasLoyalty`)
- [ ] Directives valid (`*ngIf`, `*ngFor`, `[value]`, `[checked]`)

Data loading:
- [ ] Data loaded in `ngOnInit` via existing service and route params
- [ ] Entity ID extracted from route params with null checking
- [ ] Missing ID throws error in `ngOnInit`
- [ ] No modifications to data-loading service

Read-only display:
- [ ] All fields from existing model properties (no invented)
- [ ] No properties renamed/removed/retyped
- [ ] Controls follow section 2 read-only patterns

No forms or editing:
- [ ] NO `<form>` tag
- [ ] NO `[(ngModel)]` (only `[value]`/`[checked]`)
- [ ] NO validation attributes (`required`, `name`)
- [ ] NO `<mat-error>` elements
- [ ] NO save/update/delete buttons

Controls:
- [ ] Strings: `<input matInput [value]="..." readonly />`
- [ ] Booleans: `<mat-slide-toggle [checked]="..." disabled>`
- [ ] Enums: human-readable text in readonly inputs
- [ ] Lookups: name/label in readonly inputs
- [ ] Arrays: repeatable read-only blocks with styling
- [ ] Decorative disabled icon buttons with `matPrefix`

Navigation:
- [ ] "Back" or "Back to List" button exists
- [ ] Calls existing navigation method
- [ ] No navigation methods modified/invented

Conditional sections:
- [ ] Nullable sections use computed properties
- [ ] Disabled toggles show state
- [ ] Nested fields render only when exists (`*ngIf`)
- [ ] No toggle functionality (display-only)

Child collections:
- [ ] `*ngFor` iterates items
- [ ] Each item in styled block (e.g., `.address-block`)
- [ ] All fields readonly
- [ ] NO "Add"/"Remove" buttons

Loading & error states:
- [ ] Loading spinner displays while fetching
- [ ] `*ngIf` with else clause pattern
- [ ] Errors in `.error-container`
- [ ] `isLoading` managed with `finalize()`

Enums:
- [ ] Imported and exposed to template
- [ ] Converted to human-readable text
- [ ] No names from sample without verification
- [ ] Display logic matches actual enum definition

Styling:
- [ ] No modifications to `styles.scss`/`theme.scss`
- [ ] Component SCSS includes layout-specific styles (grid, address blocks, sections)
- [ ] Global utilities used where appropriate
- [ ] Component SCSS adapted from sample when needed

## 1. Data Loading (Read-Only)

### Data loading (required):
- In `ngOnInit`, load entity data using the existing service (e.g., `getCustomerById(id)`)
- Extract entity ID from route parameters using `ActivatedRoute`
- Handle missing entity ID gracefully (throw error in `ngOnInit`)
- Display entity data in read-only format based on the returned model
- Use only properties that already exist—no additions, renames, or removals

### Loading and error states:
- Show loading spinner while data is being fetched:
  - Use `*ngIf` with else clause pattern
  - Create `<ng-template #loadingTpl>` with `<mat-progress-spinner>`
  - Set `isLoading = true` before service call, `false` in `finalize()` operator
- Display service errors prominently:
  - Use `.error-container` class for error messages
  - Example: `<div *ngIf="serviceErrors.loadError" class="error-container">...</div>`

## 2. Map Property Types to Read-Only Material Controls

All fields are **display-only** using `readonly` or `disabled` attributes. Use `[value]` or `[checked]` binding (NO `[(ngModel)]`).

| Property Type | Control |
|---------------|---------|
| String | `<input matInput [value]="model.propertyName" readonly />` |
| Boolean | `<mat-slide-toggle [checked]="model.propertyName" disabled>` |
| Enum | `<input matInput [value]="enumDisplayValue" readonly />` (convert to display text—see Enum Rules below) |
| Lookup | `<input matInput [value]="model.categoryName" readonly />` (use display name property) |
| Array | Repeatable read-only Material blocks (see section 5) |

### Visual enhancements:
- Add decorative disabled icon buttons with `matPrefix` for visual appeal
- Example: `<button mat-icon-button matPrefix disabled><mat-icon>person</mat-icon></button>`

### CRITICAL: Enum Handling

For each enum field in the model:

1. **Locate the enum definition**:
   - Use direct import path already present in component (e.g., `import { AddressType } from '...'`)
   - Otherwise use code search for `export enum AddressType`

2. **Read the enum file** and extract exact member names and values

3. **Convert to human-readable display text**:
   - Expose enum to template (e.g., `AddressType = AddressType;`)
   - Use conditional expressions for display (e.g., ternary operator for simple cases)
   - Or create helper method: `getAddressTypeDisplay(type: AddressType): string`

Forbidden: Do NOT assume enum member names from sample code. The enum definition in the target project is the source of truth.

## 3. No Forms or Validation

This is a **read-only view**, NOT a form:

### Do NOT include:
- `<form>` tags or `ngForm`
- `[(ngModel)]` two-way binding (use `[value]` or `[checked]` only)
- Validation attributes (`required`, `name`, etc.)
- `<mat-error>` elements
- Save, update, or delete buttons

## 4. Navigation Only (No Save)

There is NO save button or save functionality.

### Back button (required):
- Provide a "Back" or "Back to List" button that:
  - Calls an existing navigation method (e.g., `navigateToCustomerSearch()`)
  - Uses `mat-raised-button` with `color="accent"` and back arrow icon
  - Example: `<button mat-raised-button color="accent" (click)="navigateToCustomerSearch()"><mat-icon>arrow_back</mat-icon>Back to List</button>`

Do NOT modify or invent navigation methods.

## 5. Conditional Sections & Child Collections (Read-Only Display)

### Nullable object sections (e.g., `loyalty: LoyaltyDto | null`):
- Use computed property (e.g., `get hasLoyalty(): boolean { return !!this.model?.loyalty; }`)
- Display disabled toggle showing state: `<mat-slide-toggle [checked]="hasLoyalty" disabled>Has Loyalty</mat-slide-toggle>`
- Use `*ngIf` to conditionally render nested fields when present
- Display nested fields in readonly format
- Do NOT add logic to toggle or modify values (display-only)

### Child collections (e.g., `addresses`, `phones`):
- Render each item in styled read-only Material block (e.g., `.address-block`)
- Use `*ngFor` to iterate over items
- Display all fields as readonly inputs
- Add visual styling with background colors and borders (see sample SCSS)
- Do NOT add "Add" or "Remove" buttons
- Do NOT make collections editable

## 6. Styling Requirements

- **Use global utilities first**: `.grid`, `.grid-item`, `.grid-item-wide`, `.section-title`, `.address-block`, `.pa-4`, `.mb-4`, `.ux-gradient-primary`, etc.
- **Add component SCSS** for layout-specific styles (grid, address blocks, sections) when global styles don't provide it
- **The generated page must visually match the sample layout**
- **Add new utilities to `styles.scss`** only when reusable across multiple pages
- **NEVER modify** existing styles in `styles.scss` or `theme.scss`—only add new ones
- **Component SCSS required**: The view-entity sample relies on component SCSS for layout. Copy/adapt sample SCSS unless those exact styles exist globally. Do not leave component SCSS empty if template uses classes that aren't confirmed global.

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

## Definition of Done

### Template compilation:
- [ ] All bound properties exist in TS (e.g., `model.*`, `isLoading`, `serviceErrors.*`, enum types)
- [ ] All called methods exist in TS (e.g., `loadEntityById()`, navigation methods, computed properties like `hasLoyalty`)
- [ ] All structural directives are syntactically valid (`*ngIf`, `*ngFor`, `[value]`, `[checked]`)

### Data loading:
- [ ] Data loaded in `ngOnInit` using existing service and route parameters
- [ ] Entity ID extracted from route params with proper null checking
- [ ] Missing entity ID throws error in `ngOnInit`
- [ ] No modifications to data-loading service method

### Read-only display:
- [ ] All displayed fields come from existing model properties (no invented fields)
- [ ] No model properties renamed, removed, or retyped
- [ ] All controls follow read-only Material patterns from section 2

### No forms or editing:
- [ ] NO `<form>` tag is used
- [ ] NO `[(ngModel)]` two-way binding (only `[value]` or `[checked]`)
- [ ] NO validation attributes (`required`, `name`, etc.)
- [ ] NO `<mat-error>` elements
- [ ] NO save, update, or delete buttons

### Control implementation:
- [ ] Strings use `<input matInput [value]="..." readonly />`
- [ ] Booleans use `<mat-slide-toggle [checked]="..." disabled>`
- [ ] Enums display as human-readable text in readonly inputs
- [ ] Lookups display name/label property in readonly inputs
- [ ] Arrays render as repeatable read-only blocks with proper styling
- [ ] Decorative disabled icon buttons with `matPrefix` used for visual appeal

### Navigation:
- [ ] "Back" or "Back to List" button exists
- [ ] Button calls existing navigation method (e.g., `navigateToCustomerSearch()`)
- [ ] No navigation methods modified or invented

### Conditional sections (read-only):
- [ ] Nullable sections use computed properties (e.g., `get hasLoyalty(): boolean`)
- [ ] Disabled toggles show state of nullable sections
- [ ] Nested fields render only when section exists using `*ngIf`
- [ ] No toggle functionality (disabled toggles are display-only)

### Child collections:
- [ ] Collections use `*ngFor` to iterate items
- [ ] Each item renders in styled block (e.g., `.address-block`)
- [ ] All fields within items are readonly
- [ ] NO "Add" or "Remove" buttons

### Loading and error states:
- [ ] Loading spinner displays while data is being fetched
- [ ] `*ngIf` with else clause pattern used for loading state
- [ ] Service errors display in `.error-container` elements
- [ ] `isLoading` flag managed with `finalize()` operator

### Enums:
- [ ] Every enum used is imported and exposed to template
- [ ] Enum values converted to human-readable display text
- [ ] No enum names copied from sample without verification
- [ ] Enum display logic matches actual enum definition from codebase

### Styling:
- [ ] Existing `styles.scss` and `theme.scss` not modified (only additions if needed)
- [ ] Component SCSS includes layout-specific styles (grid, address blocks, sections)
- [ ] Global utility classes used where appropriate
- [ ] Component SCSS adapted from sample when sample uses component styling

""").WithFrontMatter(fm =>
                {
                    fm.Set("paths", @"
  - ""**/*.component.ts""
  - ""**/*.component.html""");
                });
        }

        [IntentManaged(Mode.Fully)]
        public override IMarkdownFile MarkdownFile { get; }

        [IntentManaged(Mode.Fully)]
        public override ITemplateFileConfig GetTemplateFileConfig() => MarkdownFile.GetConfig();

    }
}