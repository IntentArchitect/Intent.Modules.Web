using System;
using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.ComponentStyle;
using Intent.Modules.Angular.Templates.Component.LayoutComponentHtml;
using Intent.Modules.Angular.Templates.Component.LayoutComponentStyle;
using Intent.Modules.Angular.Templates.Component.LayoutComponentTypescript;
using Intent.Modules.Angular.Templates.Core.Main;
using Intent.Modules.Angular.Templates.Core.StylesDotScssFile;
using Intent.Modules.Common;
using Intent.Modules.Common.Html.Templates;
using Intent.Modules.Common.Plugins;
using Intent.Modules.Common.TypeScript.Templates;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;
using static System.Net.Mime.MediaTypeNames;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.Components.Material.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SampleFileConfiguration : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.Components.Material.SampleFileConfiguration";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 1000;

        protected override void OnAfterTemplateRegistrations(IApplication application)
        {
            UpdateLayoutHtml(application);
            UpdateLayoutStyle(application);
            UpdateLayoutTypescript(application);

            UpdateCoreStyles(application);

            UpdateSampleDefaults(application);
        }

        private static void UpdateLayoutHtml(IApplication application)
        {
            var layoutTemplates = application.FindTemplateInstances<LayoutComponentHtmlTemplate>(LayoutComponentHtmlTemplate.TemplateId);

            if (layoutTemplates.Any(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase)))
            {
                var mainTemplate = layoutTemplates.First(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase));
                mainTemplate.SetContent(MainLayoutDefaultHtml);
            }
        }

        private static void UpdateLayoutStyle(IApplication application)
        {
            var layoutTemplates = application.FindTemplateInstances<LayoutComponentStyleTemplate>(LayoutComponentStyleTemplate.TemplateId);

            if (layoutTemplates.Any(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase)))
            {
                var mainTemplate = layoutTemplates.First(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase));
                mainTemplate.SetContent(MainLayoutDefaulStyle);
            }
        }

        private static void UpdateLayoutTypescript(IApplication application)
        {
            var layoutTemplates = application.FindTemplateInstances<LayoutComponentTypescriptTemplate>(LayoutComponentTypescriptTemplate.TemplateId);

            if (layoutTemplates.Any(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase)))
            {
                var mainTemplate = layoutTemplates.First(t => t.LayoutName.Equals("main", System.StringComparison.CurrentCultureIgnoreCase));
                var file = mainTemplate.TypescriptFile;

                file.AddImport("MatSidenavModule", "@angular/material/sidenav");
                file.AddImport("MatToolbarModule", "@angular/material/toolbar");
                file.AddImport("MatIconModule", "@angular/material/icon");
                file.AddImport("MatButtonModule", "@angular/material/button");
                file.AddImport("MatListModule", "@angular/material/list");

                mainTemplate.AddImports("MatSidenavModule");
                mainTemplate.AddImports("MatToolbarModule");
                mainTemplate.AddImports("MatIconModule");
                mainTemplate.AddImports("MatButtonModule");
                mainTemplate.AddImports("MatListModule");
            }
        }

        private static void UpdateCoreStyles(IApplication application)
        {
            var stylesTemplate = application.FindTemplateInstance<StylesDotScssFileTemplate>(StylesDotScssFileTemplate.TemplateId);
            stylesTemplate.SetContent(CoreStyleDefaultContent);
        }

        private static void UpdateSampleDefaults(IApplication application)
        {
            var sampleComponentNames = new[]
            {
                "home",
                "examplepage"
            };

            application.FindTemplateInstances<ComponentStyleTemplate>(ComponentStyleTemplate.TemplateId)
                .Where(t => sampleComponentNames.Contains(t.ComponentName.ToLower()))
                .ToList()
                .ForEach(t =>
                {
                    t.SetContent(GetTemplateStyleContent(t.ComponentName.ToLower()));
                });
        }

        private static string GetTemplateStyleContent(string componentName) => componentName switch
        {
            "home" => HomeDefaultStyle,
            "examplepage" => ExamplePageDefaultStyle,
            _ => string.Empty
        };

        private const string MainLayoutDefaultHtml = @"
<input type=""checkbox"" id=""sidebar-toggle"" class=""sidebar-toggle"" />

<div class=""app-layout"">
  <header class=""app-header"">
    <label for=""sidebar-toggle"" class=""menu-toggle"" aria-label=""Toggle menu"">
      <svg xmlns=""http://www.w3.org/2000/svg"" width=""24"" height=""24"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"">
        <line x1=""3"" y1=""12"" x2=""21"" y2=""12""></line>
        <line x1=""3"" y1=""6"" x2=""21"" y2=""6""></line>
        <line x1=""3"" y1=""18"" x2=""21"" y2=""18""></line>
      </svg>
    </label>
    <a routerLink=""/"" class=""home-link"">
      <svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"">
        <path d=""M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z""></path>
        <polyline points=""9 22 9 12 15 12 15 22""></polyline>
      </svg>
      <span>Home</span>
    </a>
  </header>

  <aside class=""app-sidebar"">
    <nav class=""sidebar-nav"">
      <a routerLink=""/example-page/ModeledRoute"" class=""nav-item"" routerLinkActive=""active"">
        <svg xmlns=""http://www.w3.org/2000/svg"" width=""18"" height=""18"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"">
          <path d=""M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z""></path>
          <polyline points=""3.27 6.96 12 12.01 20.73 6.96""></polyline>
          <line x1=""12"" y1=""22.08"" x2=""12"" y2=""12""></line>
        </svg>
        <span>Example Page</span>
      </a>
    </nav>
  </aside>

  <main class=""app-content"">
    <router-outlet></router-outlet>
  </main>
</div>";

        private const string MainLayoutDefaulStyle = @"// Default Layout Styles. This can overridden or modified as needed manually or using AI

@use '../../../theme' as theme;

:host {
  display: block;
  height: 100vh;
  overflow: hidden;
}

.app-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 64px;
  z-index: 1001;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.15);
  background: linear-gradient(135deg, #{theme.$primary-color} 0%, #{theme.$primary-light} 100%) !important;
  display: flex;
  align-items: center;
  padding: 0 1rem;
  gap: 0.75rem;
}

.menu-toggle {
  cursor: pointer;
  padding: 0.5rem;
  display: flex;
  align-items: center;
  color: white;
  
  svg {
    display: block;
  }
}

.sidebar-toggle {
  display: none;
}

.home-link {
  color: white;
  text-decoration: none;
  padding: 0.5rem 1rem;
  display: flex;
  align-items: center;
  gap: 0.5rem;
  border-radius: 4px;
  transition: background-color 0.2s;
  
  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }

  span {
    font-size: 1.1rem;
    font-weight: 600;
  }
}

.app-layout {
  height: calc(100vh - 64px);
  margin-top: 64px;
  display: flex;
}

.app-sidebar {
  width: 260px;
  background: linear-gradient(180deg, rgba(theme.$primary-color, 0.03) 0%, rgba(theme.$primary-light, 0.05) 100%);
  border-right: 2px solid rgba(theme.$primary-color, 0.2);
  overflow-y: auto;
  overflow-x: hidden;
  z-index: 1000;
}

.sidebar-nav {
  padding-top: 1.5rem;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: 1rem;
  padding: 0.75rem 1.5rem;
  margin: 0.125rem 1rem;
  border-radius: 8px;
  color: #555;
  text-decoration: none;
  font-weight: 500;
  font-size: 1rem;
  transition: all 0.2s;

  svg {
    color: #666;
    flex-shrink: 0;
  }

  &:hover {
    background-color: rgba(theme.$primary-color, 0.08);
    color: #{theme.$primary-color};

    svg {
      color: #{theme.$primary-color};
    }
  }

  &.active {
    background-color: rgba(theme.$primary-color, 0.12);
    color: #{theme.$primary-color};
    font-weight: 600;

    svg {
      color: #{theme.$primary-color};
    }
  }
}

.app-content {
  flex: 1;
  height: 100%;
  overflow: auto;
}

@media (max-width: 768px) {
  .app-sidebar {
    position: fixed;
    left: -260px;
    top: 64px;
    height: calc(100vh - 64px);
    z-index: 1000;
    transition: left 0.3s;
  }

  .sidebar-toggle:checked ~ .app-layout .app-sidebar {
    left: 0;
  }
}";

        private const string HomeDefaultStyle = @"// Default Home Component Styles. This can overridden or modified as needed manually or using AI
@use '../../../theme' as theme;
@use '../../../styles.scss' as *;

:host {
  display: block;
  height: 100%;
  overflow: hidden;
}

.home-container {
  @extend .page-container;
}

// Override icon badge with accent color
.icon-badge {
  background: $accent-gradient;
  box-shadow: 0 8px 20px rgba(theme.$accent-color, 0.3);
}

// Override info-display to use accent color
.info-display {
  background: linear-gradient(135deg, rgba(theme.$accent-color, 0.08) 0%, rgba(theme.$accent-light, 0.12) 100%);
  border-left: 4px solid theme.$accent-color;
}

// Component-specific description override
.description {
  font-size: 1.2rem;
  color: $text-primary;

  strong {
    color: #2c2c2c;
    font-weight: 700;
  }
}

.docs-button {
  display: inline-flex;
  align-items: center;
  gap: $spacing-xs;
  background: $primary-gradient;
  color: white;
  padding: $spacing-sm $spacing-lg;
  border-radius: $radius-lg;
  font-size: 1.1rem;
  font-weight: 600;
  text-decoration: none;
  transition: all $transition-normal ease;
  box-shadow: $shadow-primary-md;

  svg {
    flex-shrink: 0;
  }

  &:hover {
    transform: translateY(-2px);
    box-shadow: $shadow-primary-lg;
    background: $primary-gradient-dark;
  }

  &:active {
    transform: translateY(0);
  }
}";

        private const string ExamplePageDefaultStyle = @"// Default Home Component Styles. This can overridden or modified as needed manually or using AI
// Default Example Component Styles. This can overridden or modified as needed manually or using AI
@use '../../../theme' as theme;
@use '../../../styles.scss' as *;

:host {
  display: block;
  height: 100%;
  overflow: hidden;
}

.example-container {
  @extend .page-container;
}

// Override icon badge with accent color (same as home page)
.icon-badge {
  background: $accent-gradient;
  box-shadow: 0 8px 20px rgba(theme.$accent-color, 0.3);
}

// Component-specific param display
.param-display {
  @extend .info-display;
  flex-wrap: wrap;
  gap: 0.5rem;
  background: linear-gradient(135deg, rgba(theme.$accent-color, 0.08) 0%, rgba(theme.$accent-light, 0.12) 100%);
  border-left: 4px solid theme.$accent-color;
}

.param-label {
  font-weight: 600;
  color: theme.$primary-color;
  font-size: 1.1rem;
}

.param-value {
  font-size: 1.5rem;
  font-weight: 700;
  color: theme.$primary-light;
  font-family: 'Courier New', monospace;
  background: white;
  padding: $spacing-xs $spacing-md;
  border-radius: $radius-md;
  box-shadow: $shadow-sm;
}";

        private const string CoreStyleDefaultContent = @"// You can add global styles to this file, and also import other style files 

@use '@angular/material' as mat;
@use './theme' as theme;

// Include Material core styles
@include mat.core();

// Include theme styles for all components
@include mat.all-component-themes(theme.$my-theme);
$success-color: #00c853;
$success-dark: #00a040;
$text-primary: #333;
$text-secondary: #555;
$text-muted: #666;

// Gradients
$background-gradient: linear-gradient(135deg, #f5f5f5 0%, #e3f2fd 100%);
$primary-gradient: linear-gradient(135deg, #{theme.$primary-color} 0%, #{theme.$primary-light} 100%);
$primary-gradient-dark: linear-gradient(135deg, #{theme.$primary-dark} 0%, #{theme.$primary-color} 100%);
$accent-gradient: linear-gradient(135deg, #{theme.$accent-color} 0%, #{theme.$accent-light} 100%);
$accent-gradient-dark: linear-gradient(135deg, #{theme.$accent-dark} 0%, #{theme.$accent-color} 100%);
$success-gradient: linear-gradient(135deg, #00c853 0%, #69f0ae 100%);
$success-gradient-dark: linear-gradient(135deg, #00a040 0%, #00c853 100%);
$info-gradient: linear-gradient(135deg, #e3f2fd 0%, #bbdefb 100%);
$sidebar-gradient: linear-gradient(180deg, rgba(25, 118, 210, 0.03) 0%, rgba(66, 165, 245, 0.05) 100%);

// Shadows
$shadow-sm: 0 2px 8px rgba(0, 0, 0, 0.1);
$shadow-md: 0 2px 8px rgba(0, 0, 0, 0.15);
$shadow-primary: 0 8px 20px rgba(25, 118, 210, 0.3);
$shadow-primary-md: 0 4px 12px rgba(25, 118, 210, 0.3);
$shadow-primary-lg: 0 6px 16px rgba(25, 118, 210, 0.4);
$shadow-accent-md: 0 4px 12px rgba(124, 77, 255, 0.3);
$shadow-accent-lg: 0 6px 16px rgba(124, 77, 255, 0.4);
$shadow-success-md: 0 4px 12px rgba(0, 200, 83, 0.3);
$shadow-success-lg: 0 6px 16px rgba(0, 200, 83, 0.4);

// Border radius
$radius-sm: 4px;
$radius-md: 6px;
$radius-lg: 8px;

// Transitions
$transition-fast: 0.2s;
$transition-normal: 0.3s;

// Spacing
$spacing-xs: 0.5rem;
$spacing-sm: 0.75rem;
$spacing-md: 1rem;
$spacing-lg: 1.5rem;
$spacing-xl: 2rem;
$spacing-2xl: 3rem;

body { 
  margin: 0;
  overflow: hidden;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}

// Common page component styles
.page-container {
  height: 100%;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  background: $background-gradient;
  padding: 3rem 2rem 0;
  overflow: hidden;
}

.content-wrapper {
  width: 100%;
  max-width: 800px;
  text-align: center;
  flex-shrink: 1;
  min-height: 0;
}

.icon-badge {
  width: 80px;
  height: 80px;
  background: $primary-gradient;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  margin: 0 auto $spacing-xl;
  color: white;
  box-shadow: $shadow-primary;
}

.page-title {
  font-size: 2.5rem;
  font-weight: 700;
  color: theme.$primary-color;
  margin: 0 0 $spacing-xl 0;
  text-align: center;
  line-height: 1.2;

  @media (max-width: 768px) {
    font-size: 2rem;
  }
}

.description {
  font-size: 1.1rem;
  color: $text-secondary;
  line-height: 1.8;
  margin-bottom: $spacing-xl;
  text-align: center;

  strong {
    font-weight: 600;
    color: theme.$primary-color;
  }

  code {
    background: #f5f5f5;
    padding: 0.2rem $spacing-xs;
    border-radius: $radius-sm;
    color: theme.$primary-color;
    font-family: 'Courier New', monospace;
    font-size: 0.95em;
  }
}

.info-box {
  background: #fff8e1;
  border-left: 4px solid #ffc107;
  padding: $spacing-lg;
  border-radius: $radius-lg;
  color: $text-muted;

  strong {
    color: #f57c00;
    display: block;
    margin-bottom: $spacing-xs;
    font-size: 1.1rem;
  }

  p {
    margin: 0;
    line-height: 1.6;
  }

  code {
    background: rgba(0, 0, 0, 0.05);
    padding: 0.2rem 0.4rem;
    border-radius: 3px;
    font-family: 'Courier New', monospace;
    font-size: 0.9em;
  }
}

.info-display {
  background: $info-gradient;
  border-left: 4px solid theme.$primary-light;
  padding: $spacing-lg;
  border-radius: $radius-lg;
  margin-bottom: $spacing-xl;
  display: flex;
  align-items: center;
  justify-content: center;
}

// Common card container styles
.card-container {
  display: flex;
  justify-content: center;
  padding: 24px;
}

.card-content {
  width: 100%;
  max-width: 960px;
  display: flex;
  flex-direction: column;
  gap: 16px;
  padding: 24px;
}

// Common error message styles
.error-message {
  color: theme.$warn-color;
  background-color: theme.$warn-light;
  border-radius: $radius-sm;
  padding: 8px 12px;
  font-size: 14px;
}

// Common loading overlay styles
.loading-overlay {
  position: absolute;
  inset: 0;
  display: flex;
  align-items: center;
  justify-content: center;
  background-color: rgba(255, 255, 255, 0.7);
}

// Common no results / empty state styles
.empty-state {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  gap: 8px;
  padding: 32px 16px;
  color: rgba(0, 0, 0, 0.54);

  mat-icon {
    font-size: 40px;
    width: 40px;
    height: 40px;
  }
}

// Common table container styles
.table-container {
  position: relative;
  overflow: auto;
  border-radius: $radius-sm;

  &.loading {
    pointer-events: none;
    opacity: 0.8;
  }
}

// Utility classes
.pa-4 { padding: 1.5rem !important; }
.mb-4 { margin-bottom: 1.5rem !important; }
.mr-1 { margin-right: 0.25rem !important; }
.mr-2 { margin-right: 0.5rem !important; }
.my-4 { margin-top: 1rem !important; margin-bottom: 1rem !important; }
.w-100 { width: 100%; }
.text-white { color: white; }
.font-weight-bold { font-weight: 600; }
.opacity-90 { opacity: 0.9; }

// Gradient header card
.ux-gradient-primary {
  background: linear-gradient(135deg, #{theme.$primary-color} 0%, #{theme.$primary-light} 100%);
  color: white;
  border: none;
  box-shadow: 0 4px 12px rgba(25, 118, 210, 0.3);
}

// Fade in animation
@keyframes fadeInUp {
  from {
    opacity: 0;
    transform: translateY(20px);
  }
  to {
    opacity: 1;
    transform: translateY(0);
  }
}

.ux-fade-in-up {
  animation: fadeInUp 0.5s ease-out forwards;
}

// Header content styling
.header-content {
  display: flex;
  flex-direction: column;
  gap: 0.5rem;
}

.title-row {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  
  mat-icon {
    font-size: 32px;
    width: 32px;
    height: 32px;
  }
}

.subtitle-row {
  font-size: 1rem;
  line-height: 1.5;
}

// Button row
.button-row {
  display: flex;
  gap: 0.5rem;
  flex-wrap: wrap;
  align-items: center;
}

// Status chips
.chip-success {
  background-color: #c8e6c9 !important;
  color: #2e7d32 !important;
}

.chip-error {
  background-color: #ffcdd2 !important;
  color: #c62828 !important;
}

// Loading container
.loading-container {
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
  padding: 3rem;
  color: rgba(0, 0, 0, 0.54);
}

.mt-2 { margin-top: 0.5rem !important; }";

    }
}