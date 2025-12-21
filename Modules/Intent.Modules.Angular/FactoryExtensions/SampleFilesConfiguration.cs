using System.Linq;
using Intent.Engine;
using Intent.Modules.Angular.Templates.Component.ComponentHtml;
using Intent.Modules.Angular.Templates.Component.ComponentStyle;
using Intent.Modules.Angular.Templates.Component.LayoutComponentHtml;
using Intent.Modules.Angular.Templates.Component.LayoutComponentStyle;
using Intent.Modules.Common;
using Intent.Modules.Common.Plugins;
using Intent.Plugins.FactoryExtensions;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FactoryExtension", Version = "1.0")]

namespace Intent.Modules.Angular.FactoryExtensions
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public class SampleFilesConfiguration : FactoryExtensionBase
    {
        public override string Id => "Intent.Angular.SampleFilesConfiguration";

        [IntentManaged(Mode.Ignore)]
        public override int Order => 0;

        /// <summary>
        /// This is an example override which would extend the
        /// <see cref="ExecutionLifeCycleSteps.AfterTemplateRegistrations"/> phase of the Software Factory execution.
        /// See <see cref="FactoryExtensionBase"/> for all available overrides.
        /// </summary>
        /// <remarks>
        /// It is safe to update or delete this method.
        /// </remarks>
        protected override void OnAfterTemplateRegistrations(IApplication application)
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

            application.FindTemplateInstances<ComponentHtmlTemplate>(ComponentHtmlTemplate.TemplateId)
                .Where(t => sampleComponentNames.Contains(t.ComponentName.ToLower()))
                .ToList()
                .ForEach(t =>
                {
                    t.SetContent(GetTemplateHtmlContent(t.ComponentName.ToLower()));
                });

            var sampleLayoutNames = new[]
            {
                "main"
            };

            application.FindTemplateInstances<LayoutComponentStyleTemplate>(LayoutComponentStyleTemplate.TemplateId)
                .Where(t => sampleLayoutNames.Contains(t.LayoutName.ToLower()))
                .ToList()
                .ForEach(t =>
                {
                    t.SetContent(GetLayoutStyleContent(t.LayoutName.ToLower()));
                });

            application.FindTemplateInstances<LayoutComponentHtmlTemplate>(LayoutComponentHtmlTemplate.TemplateId)
                .Where(t => sampleLayoutNames.Contains(t.LayoutName.ToLower()))
                .ToList()
                .ForEach(t =>
                {
                    t.SetContent(GetLayoutHtmlContent(t.LayoutName.ToLower()));
                });
        }

        private static string GetTemplateStyleContent(string componentName) => componentName switch
        {
            "home" => HomeDefaultStyle,
            "examplepage" => ExamplePageDefaultStyle,
            _ => string.Empty
        };

        private static string GetTemplateHtmlContent(string componentName) => componentName switch
        {
            "home" => HomeDefaultHtml,
            "examplepage" => ExamplePageDefaultHtml,
            _ => string.Empty
        };
        private static string GetLayoutStyleContent(string layoutName) => layoutName switch
        {
            "main" => MainLayoutDefaultStyle,
            _ => string.Empty
        };

        private static string GetLayoutHtmlContent(string layoutName) => layoutName switch
        {
            "main" => MainLayoutDefaultHtml,
            _ => string.Empty
        };

        private const string HomeDefaultStyle = @"// Default Home Component Styles. This can overridden or modified as needed manually or using AI
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
  box-shadow: 0 8px 20px rgba(124, 77, 255, 0.3);
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
  box-shadow: 0 8px 20px rgba(124, 77, 255, 0.3);
}

// Component-specific param display
.param-display {
  @extend .info-display;
  flex-wrap: wrap;
  gap: 0.5rem;
}

.param-label {
  font-weight: 600;
  color: $primary-color;
  font-size: 1.1rem;
}

.param-value {
  font-size: 1.5rem;
  font-weight: 700;
  color: $primary-light;
  font-family: 'Courier New', monospace;
  background: white;
  padding: $spacing-xs $spacing-md;
  border-radius: $radius-md;
  box-shadow: $shadow-sm;
}";

        private const string HomeDefaultHtml = @"
<!-- This template is a sample homepage for an Angular application generated by Intent Architect. -->
<!-- Update this with your own home page design and content -->
<div class=""home-container"">
  <div class=""content-wrapper"">
    <div class=""icon-badge"">
      <svg xmlns=""http://www.w3.org/2000/svg"" width=""48"" height=""48"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
        <path d=""M3 9l9-7 9 7v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2z""></path>
        <polyline points=""9 22 9 12 15 12 15 22""></polyline>
      </svg>
    </div>
    <h1 class=""page-title"">Angular Application Built with Intent Architect</h1>
    <p class=""description"">
      <strong>Model-driven development meets AI-powered automation</strong>
    </p>
    <div class=""info-display"">
      <a href=""https://docs.intentarchitect.com/articles/application-development/modelling/ui-designer/angular-modeling/angular-modeling.html"" target=""_blank"" class=""docs-button"">
        <svg xmlns=""http://www.w3.org/2000/svg"" width=""20"" height=""20"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
          <path d=""M4 19.5A2.5 2.5 0 0 1 6.5 17H20""></path>
          <path d=""M6.5 2H20v20H6.5A2.5 2.5 0 0 1 4 19.5v-15A2.5 2.5 0 0 1 6.5 2z""></path>
        </svg>
        View Documentation
      </a>
    </div>
  </div>
</div>";

        private const string ExamplePageDefaultHtml = @"
<!-- This template is an example for an Angular application generated by Intent Architect. -->
<!-- You can delete the page from the Intent Architect UI designer when no longer required -->
<div class=""example-container"">
  <div class=""content-wrapper"">
    <div class=""icon-badge"">
      <svg xmlns=""http://www.w3.org/2000/svg"" width=""48"" height=""48"" viewBox=""0 0 24 24"" fill=""none"" stroke=""currentColor"" stroke-width=""2"" stroke-linecap=""round"" stroke-linejoin=""round"">
        <path d=""M21 16V8a2 2 0 0 0-1-1.73l-7-4a2 2 0 0 0-2 0l-7 4A2 2 0 0 0 3 8v8a2 2 0 0 0 1 1.73l7 4a2 2 0 0 0 2 0l7-4A2 2 0 0 0 21 16z""></path>
        <polyline points=""3.27 6.96 12 12.01 20.73 6.96""></polyline>
        <line x1=""12"" y1=""22.08"" x2=""12"" y2=""12""></line>
      </svg>
    </div>
    <h1 class=""page-title"">Route Parameters in Action</h1>
    <p class=""description"">
      This page demonstrates route parameters modeled in Intent Architect.
    </p>
    <div class=""param-display"">
      <span class=""param-label"">Current Value:</span>
      <span class=""param-value"">{{ title }}</span>
    </div>
  </div>
</div>";

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

        private const string MainLayoutDefaultStyle = @"// Default Layout Styles. This can overridden or modified as needed manually or using AI
@use '../../../styles.scss' as *;

:host {
  display: block;
  height: 100vh;
  overflow: hidden;
}

.sidebar-toggle {
  display: none;
}

.app-layout {
  display: flex;
  flex-direction: column;
  height: 100%;
  overflow: hidden;
}

.app-header {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  height: 64px;
  background: $primary-gradient;
  box-shadow: $shadow-md;
  display: flex;
  align-items: center;
  padding: 0 $spacing-lg;
  gap: $spacing-md;
  z-index: 1000;
}

.menu-toggle {
  background: none;
  border: none;
  color: white;
  cursor: pointer;
  padding: $spacing-xs;
  display: flex;
  align-items: center;
  justify-content: center;
  border-radius: $radius-sm;
  transition: background-color $transition-fast;

  &:hover {
    background-color: rgba(255, 255, 255, 0.1);
  }

  svg {
    display: block;
  }
}

.home-link {
  display: flex;
  align-items: center;
  gap: $spacing-xs;
  color: white;
  text-decoration: none;
  font-size: 1.1rem;
  font-weight: 600;
  padding: $spacing-xs $spacing-md;
  border-radius: $radius-lg;
  transition: background-color $transition-fast;

  &:hover {
    background-color: rgba(255, 255, 255, 0.15);
  }

  svg {
    flex-shrink: 0;
  }
}

.app-sidebar {
  position: fixed;
  top: 64px;
  left: 0;
  bottom: 0;
  width: 260px;
  background: $sidebar-gradient;
  border-right: 1px solid rgba(25, 118, 210, 0.1);
  transition: transform $transition-normal ease;
  z-index: 999;
  overflow-y: auto;
}

.sidebar-toggle:checked ~ .app-layout .app-sidebar {
  transform: translateX(-260px);
}

.sidebar-toggle:checked ~ .app-layout .app-content {
  margin-left: 0;
}

.sidebar-nav {
  padding: $spacing-lg $spacing-md;
}

.sidebar-title {
  font-size: 0.875rem;
  font-weight: 700;
  color: $text-muted;
  text-transform: uppercase;
  letter-spacing: 0.5px;
  margin: 0 0 $spacing-md $spacing-sm;
}

.nav-item {
  display: flex;
  align-items: center;
  gap: $spacing-sm;
  padding: 0.875rem $spacing-sm;
  color: $text-secondary;
  text-decoration: none;
  border-radius: $radius-lg;
  font-weight: 500;
  transition: all $transition-fast;
  margin-bottom: 0.25rem;

  svg {
    flex-shrink: 0;
    color: $text-muted;
  }

  &:hover {
    background-color: rgba(25, 118, 210, 0.08);
    color: $primary-color;

    svg {
      color: $primary-color;
    }
  }

  &.active {
    background-color: rgba(25, 118, 210, 0.12);
    color: $primary-color;
    font-weight: 600;

    svg {
      color: $primary-color;
    }
  }
}

.app-content {
  margin-top: 64px;
  margin-left: 260px;
  transition: margin-left $transition-normal ease;
  flex: 1;
  overflow: hidden;
  height: calc(100vh - 64px);
}

@media (max-width: 768px) {
  .app-sidebar {
    transform: translateX(-260px);
  }

  .sidebar-toggle:checked ~ .app-layout .app-sidebar {
    transform: translateX(0);
    box-shadow: 2px 0 8px rgba(0, 0, 0, 0.15);
  }

  .app-content {
    margin-left: 0;
  }
}";
    }
}