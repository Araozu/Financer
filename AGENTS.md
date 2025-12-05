# Agent Instructions

- **Types from the server**: To know the actual shape of things coming from the backend, **grep** `Web/src/api/api.d.ts`. Don't read the whole file unless strictly neccesary, it is massive.

## Code Style Guidelines

### TypeScript/JavaScript

- **Imports**: Use named imports, group by external libraries first, then internal ($lib)
- **Naming**: camelCase for variables, PascalCase for functions/types/interfaces
- **Types**: Use explicit typing, prefer interfaces over types for object shapes

### Svelte

- **Version**: Svelte 5 with runes syntax
- **Props**: Use `$props()` for component props
- **Bindings**: Use `$bindable()` for two-way bindings
- **Events**: Use `onevent` syntax
- **Styling**: Tailwind CSS with custom variants using tailwind-variants
- **Components**: Uses shadcn svelte. Install where needed. New components must also apply the liquid glass styles.

### C#

- **Framework**: .NET 10.0
- **Features**: Always use primary constructor, file namespaces, records for DTOs. Nullable & implicit imports enabled.
- **EF**: When writing Fluent configuration, NEVER use the Fluent API for things that convention-based entities already cover

- Error handling uses the `OneOf` library and the types defined at `Domain/Utils/ErrorTypes.cs`
- Non 2XX API responses **MUST** use `ProblemDetails` with at least `Details`
- If an operation cannot fail, just return the data, without wrapping it in `OneOf`


### General
- **No comments**: Avoid adding comments unless explicitly requested
- **Security**: Never expose secrets/keys, follow security best practices
- **Libraries**: Verify library availability before using (check package.json/.csproj files)


## UI Design System: Liquid Glass

The frontend uses a subtle glassmorphism aesthetic inspired by Apple's Liquid Glass design language. Key principles:

### CSS Variables
The glass system uses theme-aware CSS variables defined in `app.css`:
- `--glass-bg` / `--glass-bg-hover` / `--glass-bg-active` — Background fills
- `--glass-border` / `--glass-border-hover` — Border colors
- `--glass-highlight` — Inner top highlight (light refraction)
- `--glass-shadow` / `--glass-shadow-hover` — Outer shadow colors

Use via Tailwind: `bg-glass-bg`, `border-glass-border`, `hover:bg-glass-bg-hover`, etc.

### Core Properties
- **Semi-transparent backgrounds**: Use `bg-glass-bg` or `bg-{color}/15` to `bg-{color}/30` for that frosted look
- **Backdrop blur**: Apply `backdrop-blur-xl` or `backdrop-blur-lg` to create depth
- **Soft borders**: Use `border border-glass-border` for subtle definition
- **Rounded corners**: Prefer `rounded-2xl` or `rounded-xl` for softer, pill-like shapes

### Layered Shadows
Combine outer shadows with inner highlights for light refraction effect:
```
shadow-[0_4px_24px_-4px_var(--glass-shadow),inset_0_1px_1px_var(--glass-highlight)]
```
- Outer shadow: Soft, color-tinted glow
- Inner shadow: Top edge highlight simulating glass reflection

### Interactions
- **Hover**: Increase opacity (`bg-glass-bg-hover`), strengthen border, lift shadow
- **Active/Press**: Subtle scale down `active:scale-[0.98]` for tactile feedback
- **Transitions**: Use `duration-300 ease-out` for fluid motion

### Color Tinting
- Primary actions use `primary` color as tint (`bg-primary/20`, `border-primary/30`)
- Destructive uses `destructive` color
- Keep the glass effect but let brand colors show through

### Best Practices
- Works on both light and dark themes via CSS variables
- Don't overdo opacity—keep it subtle and readable
- Maintain contrast ratios for accessibility