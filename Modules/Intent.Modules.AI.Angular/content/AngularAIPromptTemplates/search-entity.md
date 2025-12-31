
### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- Component `.scss` files should remain minimal - only add truly component-specific styles
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed

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

### 5. Using existing component methods
- (IMPORTANT) If the component already includes a method like:
  - `loadCustomers(...)`
  - `searchCustomers(...)`
  - `refreshTable(...)`
  
  You **must call that method** — not duplicate logic and not rewrite backend calls.

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
