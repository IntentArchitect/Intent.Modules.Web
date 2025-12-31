using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.StylesDotScssFile
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class StylesDotScssFileTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            if (!string.IsNullOrWhiteSpace(_content))
            {
                return _content;
            }

            return $@"// You can add global styles to this file, and also import other style files 

// Forward theme variables so they're available to files that import this file
@forward 'theme.scss';
@use './theme.scss' as *;

body {{ 
  margin: 0;
  overflow: hidden;
  font-family: -apple-system, BlinkMacSystemFont, 'Segoe UI', 'Roboto', 'Oxygen', 'Ubuntu', 'Cantarell', 'Fira Sans', 'Droid Sans', 'Helvetica Neue', sans-serif;
  -webkit-font-smoothing: antialiased;
  -moz-osx-font-smoothing: grayscale;
}}

// Common page component styles
.page-container {{
  height: 100%;
  display: flex;
  align-items: flex-start;
  justify-content: center;
  background: $background-gradient;
  padding: 3rem 2rem 0;
  overflow: hidden;
}}

.content-wrapper {{
  width: 100%;
  max-width: 800px;
  text-align: center;
  flex-shrink: 1;
  min-height: 0;
}}

.icon-badge {{
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
}}

.page-title {{
  font-size: 2.5rem;
  font-weight: 700;
  color: $primary-color;
  margin: 0 0 $spacing-xl 0;
  text-align: center;
  line-height: 1.2;

  @media (max-width: 768px) {{
    font-size: 2rem;
  }}
}}

.description {{
  font-size: 1.1rem;
  color: $text-secondary;
  line-height: 1.8;
  margin-bottom: $spacing-xl;
  text-align: center;

  strong {{
    font-weight: 600;
    color: $primary-color;
  }}

  code {{
    background: #f5f5f5;
    padding: 0.2rem $spacing-xs;
    border-radius: $radius-sm;
    color: $primary-color;
    font-family: 'Courier New', monospace;
    font-size: 0.95em;
  }}
}}

.info-box {{
  background: #fff8e1;
  border-left: 4px solid #ffc107;
  padding: $spacing-lg;
  border-radius: $radius-lg;
  color: $text-muted;

  strong {{
    color: #f57c00;
    display: block;
    margin-bottom: $spacing-xs;
    font-size: 1.1rem;
  }}

  p {{
    margin: 0;
    line-height: 1.6;
  }}

  code {{
    background: rgba(0, 0, 0, 0.05);
    padding: 0.2rem 0.4rem;
    border-radius: 3px;
    font-family: 'Courier New', monospace;
    font-size: 0.9em;
  }}
}}

.info-display {{
  background: $info-gradient;
  border-left: 4px solid $primary-light;
  padding: $spacing-lg;
  border-radius: $radius-lg;
  margin-bottom: $spacing-xl;
  display: flex;
  align-items: center;
  justify-content: center;
}}";
        }
    }
}