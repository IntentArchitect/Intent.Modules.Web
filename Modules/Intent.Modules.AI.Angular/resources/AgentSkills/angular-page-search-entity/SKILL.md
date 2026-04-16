---
name: angular-page-search-entity
description: Creates Angular search screens with grid and optional filtering. Use when building a search/list component for an entity — generates an Angular Material table, maps DTO properties to correct form controls (text, select, datepicker), enforces filters derived only from the backend service request model, and wires up search/paging behaviour using existing component methods. DO NOT USE for detail/edit forms, new entity creation flows, or non-Angular projects.
paths:
  - "**/*.component.ts"
  - "**/*.component.html"
---

## Reference Examples

Before generating any output, you MUST read all of the following skill asset files using the skill asset/file access mechanism, not the application workspace file tools.
These files are bundled with the skill and are not part of the target repository:

1. **`search-entity-sample.ts`** — MUST be read and used as the TypeScript structure template (imports, `@IntentMerge()` decorator, state fields, paging/sort state, dialog wiring, `refreshX()` on `ngOnInit`) and adapt names and types for the target entity.
2. **`search-entity-sample.html`** — MUST be read and used as the HTML layout template (gradient header card, `filter-grid`, `button-row`, `mat-table` with `matSort`) and adapt columns, bindings, and labels for the target entity.
3. **`search-entity-sample.scss`** — MUST be read whenever implementing the component, even if you expect minimal styling.
4. **`search-entity-delete-dialog.ts`** + **`search-entity-delete-dialog.html`** — MUST be read if a delete dialog is generated. Copy the `@Inject(MAT_DIALOG_DATA)` / `MatDialogRef` / `onCancel()` / `onConfirm()` pattern and adapt to the target entity's delete service call.

---

### Required modification workflow for existing components
1. Read the existing target `.ts` file.
2. Identify all existing methods that call services, dialogs, or navigation.
3. Preserve those methods unchanged.
4. Only then adapt the HTML and add minimal supporting TS code.
5. Prefer patching over full-file overwrite for existing `.ts` files.

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- SCSS decision rule:
  - Use global utilities where possible.
  - If the sample uses component SCSS for structural layout (grid columns, responsive behavior) and global styles don’t already guarantee it, copy/adapt the sample SCSS into the component SCSS.
  - Only add to styles.scss when the style is reusable across multiple pages; otherwise keep it component-scoped.
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed
- Layout parity rule: The rendered layout must match the sample at common breakpoints (mobile + desktop). If global styles do not produce the sample layout (e.g., filter fields stacking unexpectedly), the agent MUST add the minimal SCSS necessary (component-local preferred) to achieve parity.
- Filter grid requirement: .filter-grid-row-2 must render as 2 columns on desktop widths (e.g. ≥ 1024px) and 1 column on small screens. If the global .filter-grid-row-2 does not enforce this, the agent must implement it in the component SCSS (or add a new global utility class and use it).
- Global style verification: When using global classes like .filter-grid*, .button-row, .table-wrapper, the agent MUST read src/styles.scss to confirm their behavior before deciding to omit component SCSS.

### 1. Criteria must come ONLY from the backend search service
- The search form **must only expose filters that are supported in the backend search service request model**.
- Look at the generated TypeScript service proxies in `service-proxies/**`:
  - Identify the primary search method (e.g. `getCustomers`, `getOrders`, etc.).
  - Inspect the request DTO or parameters (e.g. `GetCustomersQuery`).
  - These properties are the **single source of truth** for all search fields.

- (IMPORTANT) Never invent filters:
  - No adding `fromDate`, `status`, `minAmount`, etc. unless they exist in the DTO.
  - No modifying service signatures or DTOs to make UI filters work.

### 2. Paging & Sorting Rules
- Do **not** create UI controls for paging or sorting parameters.
- If the service has parameters like `pageNo`, `pageSize`, or `orderBy`:
  - Use them within the table component.
  - Do **not** expose them as form inputs.

### 3. Mapping DTO properties to Angular Material UI controls
Choose the correct control type based on the property in the DTO (excluding paging/sorting):

- **string / string | null**
  - If named like `search`, `searchTerm`, `keyword` → Use a **single search text field**.
  - Otherwise → Use a normal `<mat-form-field>` text input.

- **boolean / boolean | null**
  - Use `<mat-select>` with:
    - All (null or empty)
    - Yes / No (or Active / Inactive depending on naming)

- **enum or lookup values**
  - Use `<mat-select>`.
  - Populate options **only from real lookup services that exist** in `service-proxies/**`.
  - Do **not** create fake lookup data.

- **number / number | null**
  - Use `<input type="number">`.

- **dates**
  - Use `mat-datepicker` when applicable.

### 4. Search button behavior
- Do not auto-query on every keystroke.
- Provide a **Search** button that:
  - Reads current form values.
  - Calls the existing component method for data loading (e.g. `loadCustomers()`).
  - Does **not** modify backend-calling methods.
- Pressing Enter in the main search field should trigger the same search behavior.
- If an “Add Entity” navigation method exists in the component (e.g. navigateToCustomerAddPage()), render an Add button in the .button-row and it MUST use:
  - `mat-raised-button`
  - `color="accent"`
  - an icon (e.g. `person_add`, `add`, etc.)
  - be placed in the same .button-row as the Search button
- If no add navigation method exists in TS, do not render an Add button (even if routes/navigation items exist).
That’s all you need

### 5. Using existing component methods
- (IMPORTANT) If the component already includes a method like:
  - `loadCustomers(...)`
  - `searchCustomers(...)`
  - `refreshTable(...)`
  
  You **must call that method** — not duplicate logic and not rewrite backend calls.

  - If methods such as `addX`, `editX`, `loadX`, `navigateToX`, `save`, `update`, or `deleteX` already exist:
  - preserve their bodies exactly
  - bind the UI to those methods
  - do not stub, clear, or reimplement them

- If an existing action method already exists in the component and the template requires supporting presentational state to render that action, the agent MAY patch that presentational state.
- Examples of allowed supporting changes:
    - adding 'actions' to displayedColumns
    - adding a simple readonly array or label map used only by the template
    - adding wrapper/helper methods that only call existing methods
    - These changes must not alter business logic, service calls, routing targets, DTOs, or payloads.

### 6. Table output rules
- Columns must represent only fields that exist on the returned DTO.
- Never invent table columns.
- Use Angular Material table (`mat-table`) or a clean equivalent template/table layout.

### 7. General constraints
- DO NOT change, add, rename, or remove DTO properties.
- DO NOT modify backend-calling methods — only call them.
- DO NOT generate UI fields for properties that do not exist in the search DTO.
- Add an “Add Entity” button **only if the TS class already defines a navigation method**, never invent one.
- (CRITICAL) If a navigation method exists in the TS class for adding entities, create the button based on that method — DO NOT create duplicate buttons based on navigation items.

### CRITICAL: Preserve existing TypeScript implementations
When the target `.ts` file already exists, you MUST treat all existing method bodies as source-of-truth implementation.

- NEVER overwrite the entire `.ts` file unless the user explicitly requests a full rewrite.
- ALWAYS read the target `.ts` file first and preserve all existing methods, fields, decorators, and constructor logic.
- You MAY add:
  - missing Angular imports
  - standalone component `imports` metadata
  - new state fields
  - computed getters
  - lifecycle wiring such as `ngOnInit()` calls
  - helper/orchestration methods that only call existing methods
- You MUST NOT:
  - remove existing logic from methods
  - replace existing method bodies with empty implementations
  - rewrite existing service-calling methods
  - rewrite existing router/navigation methods
  - delete existing properties or constructor injections unless explicitly requested

The agent MAY patch non-business, presentational TypeScript state required to render valid UI bindings for already-existing component actions. This includes items such as displayedColumns, simple UI-only arrays, and helper getters. For Angular Material tables, if an existing row action method is present, the agent SHOULD add an actions column in both the template and displayedColumns when needed, while preserving all existing service, dialog, and navigation method bodies unchanged.

If a sample file conflicts with the existing target component, the existing target component wins. Use the sample only as a layout and structure reference.

### 8. No-Filter Screens
- If the backend search/list service exposes no real filter parameters beyond loading the list, the UI MUST NOT invent or substitute non-filter content in the filter section.
- Do NOT add summary cards, informational blocks, statistics, placeholders, or decorative replacements to fill the filter area.
- In this case, preserve the overall page structure from the sample, but:
    - omit filter controls entirely, or
    - render an empty filter section only if required for layout consistency.
- The .button-row should still contain the Search button.
- Render only UI elements that correspond to:
    - actual backend-supported filters,
    - existing TypeScript action methods,
    - actual DTO fields shown in the results table.

#### Anti Over-Adaptation Rule
- When adapting a sample page, treat the sample as a layout reference only.
- Do NOT create substitute UI elements just to preserve visual symmetry.
- If the target backend/service model is simpler than the sample, the generated UI must also be simpler.

#### Forbidden When No Filters Exist
- Do NOT add:
    - summary tiles
    - count cards
    - “data source” text
    - help text blocks
    - fake search boxes
    - placeholder filters unless the target TypeScript or backend contract explicitly requires them.

### 9.Do not under-implement
- When an existing component already supports a valid action through an existing method, the agent should surface that action in the UI unless doing so would require business-logic changes.
- Missing only presentational wiring is not a valid reason to omit the action.

### 10. CRITICAL: PagedResult / list response shape verification (NO GUESSING)
Before generating any table bindings, you MUST:

- Locate the exact return type used by the component (e.g. PagedResult<CustomerSummaryDto>).
- Read the source file that defines that type (e.g. service-proxies/models/paged-result.ts).
- Use ONLY the properties that exist on that model for:
    - the table data source (e.g. results, data, value, etc.—whatever the model actually contains)
    - the paginator length binding (e.g. totalCount, total, etc.)

Forbidden:
- Do NOT assume the list property is named items.
- Do NOT invent properties for paging (no items, results, total, etc.) unless confirmed in the read model file.

Enforcement:
- If the list property name cannot be confirmed by reading the model file, STOP and request the file / clarification instead of generating bindings.

### 11. Definition of done checklist
- [] Filters render side-by-side on desktop and stack on mobile
- [] Table wrapper + loading overlay display correctly
- [] No duplicated global utility CSS unless necessary
- [] Component SCSS present if needed to reach sample parity
- [] Add button (when present) uses mat-raised-button color="accent" and is in the .button-row
- [] Existing TypeScript method bodies were preserved exactly
- [] No existing service/navigation/dialog method was emptied or rewritten
- [] Existing `.ts` file was patched, not rewritten, unless explicitly requested