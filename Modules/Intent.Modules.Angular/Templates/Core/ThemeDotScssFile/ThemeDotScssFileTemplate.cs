using System;
using System.Collections.Generic;
using Intent.RoslynWeaver.Attributes;

[assembly: DefaultIntentManaged(Mode.Fully)]
[assembly: IntentTemplate("Intent.ModuleBuilder.Templates.FileTemplateStringInterpolation", Version = "1.0")]

namespace Intent.Modules.Angular.Templates.Core.ThemeDotScssFile
{
    [IntentManaged(Mode.Fully, Body = Mode.Merge)]
    public partial class ThemeDotScssFileTemplate
    {
        [IntentManaged(Mode.Fully, Body = Mode.Ignore)]
        public override string TransformText()
        {
            // if content explicitly set, return that. The factory extensions which set the content
            // should set the content to itself if there was nothing to update
            if (!string.IsNullOrWhiteSpace(_content))
            {
                return _content;
            }

            // return existing content if the file already exists
            if (TryGetExistingFileContent(out var existingFileContent))
            {
                return existingFileContent;
            }

            // return default content
            return @$"// You can add theme styles to this file, and also import other style files
// Theme Variables - Colors, Gradients, Shadows, and Design Tokens

// Primary Colors
$primary-color: #1976d2;
$primary-dark: #1565c0;
$primary-light: #42a5f5;

// Accent Colors
$accent-color: #7c4dff;
$accent-dark: #6200ea;
$accent-light: #b388ff;

// Success Colors
$success-color: #00c853;
$success-dark: #00a040;

// Text Colors
$text-primary: #333;
$text-secondary: #555;
$text-muted: #666;

// Gradients
$background-gradient: linear-gradient(135deg, #f5f5f5 0%, #e3f2fd 100%);
$primary-gradient: linear-gradient(135deg, #1976d2 0%, #42a5f5 100%);
$primary-gradient-dark: linear-gradient(135deg, #1565c0 0%, #1976d2 100%);
$accent-gradient: linear-gradient(135deg, #7c4dff 0%, #b388ff 100%);
$accent-gradient-dark: linear-gradient(135deg, #6200ea 0%, #7c4dff 100%);
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

// Border Radius
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
";
        }
    }
}