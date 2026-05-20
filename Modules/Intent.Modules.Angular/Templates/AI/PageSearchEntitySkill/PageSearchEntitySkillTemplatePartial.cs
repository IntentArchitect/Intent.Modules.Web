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

namespace Intent.Modules.Angular.Templates.AI.PageSearchEntitySkill
{
    [IntentManaged(Mode.Merge, Signature = Mode.Fully)]
    public class PageSearchEntitySkillTemplate : MarkdownBaseTemplate<object>, IMarkdownFileBuilderTemplate
    {
        [IntentManaged(Mode.Fully)]
        public const string TemplateId = "Intent.Angular.AI.PageSearchEntitySkillTemplate";

        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public PageSearchEntitySkillTemplate(IOutputTarget outputTarget, object model = null) : base(TemplateId, outputTarget, model)
        {
            WithContentHashing = true;
            MarkdownFile = new MarkdownFile($"SKILL", "md", "angular-page-search-entity")
                .FromMarkdown($$"""
---
name: angular-page-search-entity
description: Creates Angular search screens with grid and optional filtering. Use when building a search/list component for an entity — generates an Angular Material table, maps DTO properties to correct form controls (text, select, datepicker), enforces filters derived only from the backend service request model, and wires up search/paging behaviour using existing component methods. DO NOT USE for detail/edit forms, new entity creation flows, or non-Angular projects.
template-id: {{TemplateId}}
---

## MANDATORY: Read Samples Before Implementation

STOP - You MUST read ALL sample files in the SAME folder as this SKILL.md before writing ANY code:

1. `search-entity-sample.ts`
2. `search-entity-sample.html`
3. `search-entity-sample.scss`
4. `search-entity-delete-dialog.ts` (if delete functionality needed)
5. `search-entity-delete-dialog.html` (if delete functionality needed)

Then read target component `.ts` file and related project files (models, enums, lookups, services, styles).

If any sample file cannot be accessed: Stop immediately, confirm SKILL.md folder location, retry from that location. If still inaccessible, report which file and ask user. Do NOT proceed with partial implementation or approximation.

## Preserve Existing Implementation

Read target `.ts` first. Preserve all existing service/dialog/navigation methods unchanged.

### You MAY add:
- Angular imports, standalone `imports` metadata
- UI state fields (`displayedColumns`, UI-only arrays)
- Helper methods that only call existing methods
- Lifecycle wiring (`ngOnInit()` calls)

### You MUST NOT:
- Overwrite entire `.ts` file (patch only, unless full rewrite explicitly requested)
- Remove/replace existing method bodies
- Rewrite service-calling, routing, or dialog methods
- Modify backend DTOs or service signatures
- Empty existing action methods

Sample conflicts with target component? Target wins. Sample is layout reference only.

### Exceptions

If task context states an operation opens a dialog and a matching method exists on the component, implement the method to open the dialog (after reading dialog + pattern) and add the corresponding UI button/action. Do not leave the method empty.

### Actions: TypeScript is the source of truth (even if attached HTML exists)

If the target component class contains any public row-level action/navigation methods (e.g. navigateTo*View*, navigateTo*Edit*, delete*, remove*, open*, download*), the agent MUST update the list page HTML to surface those actions in the table (typically in an actions column), even if:
- the provided/attached .html does not yet include the button, or
- the sample template does not show that action.

## 1. Filters: Backend Service Contract Only

All filters MUST exist in backend search service request DTO.

Verify: Read `service-proxies/**` → find search method → inspect request DTO (e.g., `GetCustomersQuery`) → use ONLY those properties.

Forbidden: Invent filters, modify service signatures, expose paging/sorting (`pageNo`, `pageSize`, `orderBy`) as UI controls.

## 2. Map DTO Properties to Controls

| DTO Type | Control |
|----------|---------|
| `string` (named `search`/`searchTerm`/`keyword`) | Single search text field |
| `string` / `string \| null` | `<mat-form-field>` text input |
| `boolean` / `boolean \| null` | `<mat-select>` All/Yes/No |
| Enum or lookup | `<mat-select>` with **real** lookup services (no fake data) |
| `number` / `number \| null` | `<input type="number">` |
| Date | `mat-datepicker` |

## 3. Buttons

Search: Reads form, calls existing load method (e.g., `loadCustomers()`). Enter key triggers same. No auto-query on keystroke.

Add (conditional): Only if TS has add navigation method (e.g., `navigateToCustomerAddPage()`). Use `mat-raised-button color="accent"` with icon in `.button-row`. Do NOT invent methods or duplicate buttons.

## 4. Styling

- **Global utilities first**: `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`
- **Component SCSS**: Add only if global utilities don't match sample layout
- **Layout parity**: Match sample at desktop/mobile breakpoints
- **Filter grid**: `.filter-grid-row-2` = 2 columns desktop (≥1024px), 1 column mobile
- **Before omitting component SCSS**: Read `src/styles.scss` to verify
- **NEVER modify** `styles.scss` or `theme.scss` (add only if reusable across pages)

## 5. Table Bindings

Columns: Show ONLY fields on returned DTO. Never invent.

PagedResult verification (NO GUESSING):
1. Locate return type (e.g., `PagedResult<CustomerSummaryDto>`)
2. Read definition file (e.g., `service-proxies/models/paged-result.ts`)
3. Use ONLY actual properties for data source (`results`/`data`/`value`) and paginator (`totalCount`/`total`)

Forbidden: Assume property names. Can't confirm? STOP and request file.

## 6. No-Filter Screens

If backend has no filter parameters:

MUST NOT: Invent summary cards, info blocks, statistics, placeholders, decorative replacements.

SHOULD: Keep overall structure, omit filter controls (or empty section), keep `.button-row` with Search. Render ONLY UI for actual backend filters, existing TS methods, actual DTO table fields.

Anti Over-Adaptation: Simpler backend = simpler UI. No substitute elements for visual symmetry.

## 7. Surface Existing Actions" with:
Surface Existing Actions (Mandatory UI Wiring Rule)
If the component TypeScript contains any public row-level action/navigation methods (e.g., `navigateTo*View*`, `navigateTo*Edit*`, `delete*`, `remove*`, `open*`), you are explicitly allowed to surface them in the grid by adding an actions column and/or adding additional buttons to an existing actions column.

This is allowed even when:
- the provided template does not include that specific button yet, or
- the sample template does not show that action.

This is not considered "inventing UI"; it is considered "wiring existing capabilities."

Constraints (must follow):
- Only bind to methods that already exist on the component class.
- Do not create new service calls, new router logic, or modify method internals to support the button.
- Prefer mat-icon-button with matTooltip.
- If an actions column already exists, extend it; otherwise add one.
- Keep actions minimal and in the table row (no row-click substitution unless requested).

The agent is explicitly allowed to modify the target page's .html to add missing action buttons required by TS methods.

### Attached Templates Are Layout References, Not Action Authority
If an attached/provided template conflicts with the TS-defined actions, the agent should treat the attached template as a layout baseline but must adjust it to include the TS-defined actions. 

## 8. Optional Dialog Discovery for Empty Actions (View/Edit/Delete)
When the list-page component contains a public action method (e.g., viewProduct(id)) that is empty or not wired in the template:

- Do not invent dialog components or data contracts.
- Attempt discovery:
    - Search for dialog components whose names match the intent and entity, e.g.:
        - *View*Dialog*, *ViewEntityDialog*, *Product*View*
    - Also search for existing usages of MatDialog.open( with candidate components to infer:
        - data shape ({ productId }, { id }, { entityId }, etc.)
        - expected width/disableClose patterns
- If a matching dialog component is found and its contract can be confirmed:
    - Import it into the page component and implement the method by opening the dialog using the confirmed data contract.
    - Add a row-level action button in the table to call the method.

- If no matching dialog is found OR the contract cannot be confidently verified:
    - Leave the method as-is (or no-op if required for linting), and do not add a UI button for it.
    - Do not create placeholder dialogs or guess data payloads.

## Definition of Done

- [ ] Filters: side-by-side desktop, stacked mobile
- [ ] Table wrapper + loading overlay correct
- [ ] Component SCSS present if needed for sample parity
- [ ] Add button (if present): `mat-raised-button color="accent"` in `.button-row`
- [ ] Existing TS methods preserved exactly
- [ ] `.ts` patched, not rewritten (unless requested)
- [ ] Table bindings verified against actual DTO model
- [ ] All filters from actual backend request DTO
- [ ] Table includes an actions column/buttons for all existing public navigateTo*/edit/view/delete methods (when applicable).
- [ ] All row-level public actions in the component TS are surfaced in the grid: for every public method matching `navigateTo*`, `view*`, `open*`, `edit*`, `update*`, `delete*`, `remove*` that can be bound from the row DTO (e.g., row.id), the table includes an `actions` column with a `mat-icon-button` wired to that method; no missing eligible actions and no bindings to non-existent methods/fields.

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