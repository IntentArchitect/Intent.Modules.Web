### Dialog Rules

- This component is a Material dialog, not a page. Use the standard Angular Material dialog pattern.
- The component must inject `MatDialogRef<COMPONENT_NAME>` in the constructor, and use it to close the dialog:
  - `constructor(private dialogRef: MatDialogRef<COMPONENT_NAME>, ...) { }`

- If input data is needed, also inject `@Inject(MAT_DIALOG_DATA) public data: SomeDataType`.

- Implement two top-level methods for the template to use:
  - `save()`:
    - calls the existing service method (e.g. `createCustomer()` or `updateCustomer()` logic)
    - on success, calls `this.dialogRef.close(true)`
    - on error, sets an error string but does not close the dialog

  - `cancel()`:
    - calls `this.dialogRef.close(null)` (or `false`)
    - do not reset the model or call any services here

- (IMPORTANT) In the HTML, bind the action buttons to `save()` and `cancel()`, not to raw service methods:
  - Save button: `(click)="save()"`
  - Cancel button: `(click)="cancel()"`

- (IMPORTANT) Never treat `cancel()` as a "reset the form" method. It must only close the dialog.
- (IMPORTANT) After a successful save, always close the dialog via `dialogRef.close(...)` so the caller can react (e.g. refresh the list).

- Do not call service methods directly from the template (e.g. `(click)="createCustomer()"`).
  - Always use a `save()` method that:
    - validates (if needed),
    - calls the service, and
    - closes the dialog on success.

- If a method name like `createCustomer()` or `updateCustomer()` already exists and calls the backend, either:
  - call it from inside `save()`, **or**
  - inline its logic into `save()`, but do not change its behavior to stop calling the service.


### Form & Validation in Dialogs

- Use **template-driven forms** with `FormsModule` and `ngForm`.
- Wrap the dialog content and actions in a single `<form #form="ngForm" novalidate>` element.
- Use either `(ngSubmit)="onSave(form)"` on the form with a `type="submit"` Save button, or `(click)="onSave(form)"` with `form` passed as argument. Preferred: `(ngSubmit)="onSave(form)"`.
- For each required field:
  - Add `name="..."`, `[(ngModel)]="..."`, and `#fieldCtrl="ngModel"`.
  - Add `<mat-error>` bound to `fieldCtrl.invalid && (fieldCtrl.touched || form.submitted)`.
  - Example:

```html
<mat-form-field>
    <mat-label>Name</mat-label>
    <input matInput required name="name" [(ngModel)]="model.name" #nameCtrl="ngModel" />
    <mat-error *ngIf="nameCtrl.invalid && (nameCtrl.touched || form.submitted)">
    Name is required
    </mat-error>
</mat-form-field>
```

- In `onSave(form: NgForm)`:
  - If `form.invalid`, call `form.control.markAllAsTouched()` and **return without calling any service**.
  - Only call the backend service if the form is valid.
- The Save button must be disabled if the form is invalid or a save is in progress:

```html
	<button
		mat-raised-button
		color="primary"
		type="submit"
		[disabled]="form.invalid || isLoading">
		Save
	</button>
```

  - When the user clicks Save:
  - Run `onSave(form: NgForm)`.
  - If the form is invalid:
    - Do not call any service.
    - Mark all controls as touched so `mat-error` messages are visible.
  - If the form is valid:
    - Call the existing service method (e.g. `customersService.createCustomer(...)` or `customersService.updateCustomer(...)`).
    - On success, call `this.dialogRef.close(true)` so the caller can refresh data.
    - On error, set a `serviceErrors.*` message and keep the dialog open.
- Never call `dialogRef.close(...)` on failure.
- The Cancel button must call `cancel()` and `cancel()` must only call `this.dialogRef.close(null)` (or `false`), with no additional logic.

### Styling Rules
- Use existing utility classes from `styles.scss` (e.g., `.filter-grid`, `.button-row`, `.table-wrapper`, `.ux-gradient-primary`, `.pa-4`, `.mb-4`, etc.)
- Component `.scss` files should remain minimal - only add truly component-specific styles
- If you need a new utility class or pattern that doesn't exist, you may add it to `styles.scss`
- NEVER modify existing styles in `styles.scss` or `theme.scss` - only add new ones if needed