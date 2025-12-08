async function execute() {
    var _a, _b, _c;
    const providerModelsResult = await getAiProviderModels();
    const settingName = "AI.Angular";
    let promptTemplatesString = await executeModuleTask("Intent.Modules.Common.AI.Tasks.GetPromptTemplates", application.id, "Intent.Modules.AI.Angular.Generate", element.getParent().getName() + "/" + element.getName());
    let promptTemplates = JSON.parse(promptTemplatesString);
    let defaultPromptTemplate = promptTemplates.find(t => t.recommenedDefault);
    // Open a dialog for the user to enter an AI prompt
    let promptResult = await dialogService.openForm({
        title: "AI: Implement " + element.getName(),
        icon: Icons.AiAngular,
        fields: [
            {
                id: "prompt",
                fieldType: "textarea",
                label: "Provide any additional context",
                placeholder: "Leave blank if you wish to provide no additional context.",
                hint: "This additional context will be combined with the pre-engineered prompt to guide the AI Agent.",
                value: defaultPromptTemplate === null || defaultPromptTemplate === void 0 ? void 0 : defaultPromptTemplate.defaultUserPrompt
            },
            {
                id: "templateId",
                fieldType: "select",
                label: "Prompt Template",
                placeholder: "Select Template",
                selectOptions: promptTemplates,
                hint: "Select a Prompt Template to guide the the LLM.",
                value: defaultPromptTemplate === null || defaultPromptTemplate === void 0 ? void 0 : defaultPromptTemplate.id
            },
            {
                id: "exampleComponentIds",
                fieldType: "multi-select",
                label: "Example Components",
                placeholder: "Select components",
                selectOptions: lookupTypesOf("Component").filter(x => x.id != element.id).map(x => {
                    return {
                        id: x.id,
                        description: x.getName(),
                        icon: x.getIcon(),
                        additionalInfo: x.getParents().map(x => x.getName()).join("/")
                    };
                }),
                hint: "Provide the LLM with examples of existing components that it should base its implementation on."
            },
            ...await getAiModelSelectionFields(providerModelsResult, settingName)
        ],
        submitButtonText: "Execute",
        minWidth: "750px"
    });
    // Check if the user cancelled
    if (!promptResult) {
        return;
    }
    const { providerId, modelId, thinkingLevel: thinkingLevel } = await collectAndPersistAiSettingsFromPromptResult(promptResult, providerModelsResult, settingName);
    await launchHostedModuleTask("Intent.Modules.AI.Angular.Generate", [
        application.id,
        element.id,
        (_a = promptResult.prompt) !== null && _a !== void 0 ? _a : "",
        (_c = JSON.stringify((_b = promptResult.exampleComponentIds) !== null && _b !== void 0 ? _b : [])) !== null && _c !== void 0 ? _c : "",
        providerId,
        modelId,
        thinkingLevel,
        promptResult.templateId
    ], {
        taskName: "AI: Angular for " + element.getName()
    });
}
async function getAiProviderModels() {
    const moduleTaskResult = await executeModuleTask("Intent.Modules.Common.AI.Tasks.ProviderModelsTask");
    const providerModels = JSON.parse(moduleTaskResult);
    const modelLookup = providerModels.reduce((acc, item) => {
        acc[`${item.providerId}--${item.modelName}`] = item;
        return acc;
    }, {});
    return { providerModels, modelLookup };
}
async function getAiModelSelectionFields(providerModelsResult, aiSettingKeyPrefix) {
    var _a;
    const globalSettings = await userSettings.loadGlobalAsync();
    const settingModelId = globalSettings.get(`${aiSettingKeyPrefix}.ModelId`);
    const settingThinkingLevel = globalSettings.get(`${aiSettingKeyPrefix}.ThinkingLevel`);
    const { providerModels, modelLookup } = providerModelsResult;
    const initialThinkingType = (_a = modelLookup[settingModelId]) === null || _a === void 0 ? void 0 : _a.thinkingType;
    return [
        {
            id: "model",
            fieldType: "select",
            label: "Model",
            isRequired: true,
            hint: getModelHint(providerModels, initialThinkingType),
            selectOptions: Object.entries(modelLookup)
                .map(([key, value]) => {
                return {
                    id: key,
                    description: value.modelName,
                    additionalInfo: value.providerName
                };
            }),
            value: settingModelId,
            onChange: async (config) => {
                const curThinkingType = modelLookup[config.getField("model").value].thinkingType;
                const thinkingField = config.getField("thinking");
                thinkingField.isHidden = curThinkingType === "None";
                thinkingField.selectOptions = getApplicableThinkingOptions(curThinkingType);
                thinkingField.hint = getModelHint(providerModels, curThinkingType);
                if (curThinkingType === "ThinkingLevels") {
                    thinkingField.value = "low";
                }
                else if (curThinkingType === "Unknown") {
                    thinkingField.value = "none";
                }
                else {
                    thinkingField.value = null;
                }
            }
        },
        {
            id: "thinking",
            fieldType: "select",
            label: "Thinking/reasoning mode",
            isHidden: settingThinkingLevel == null || providerModels.length === 0,
            value: settingThinkingLevel,
            selectOptions: getApplicableThinkingOptions(initialThinkingType)
        }
    ];
    function getModelHint(providerModels, thinkingType) {
        if (providerModels.length === 0) {
            return "Not seeing any AI Models? Learn how to configure or add models [here](https://docs.intentarchitect.com/articles/modules-common/intent-common-ai/intent-common-ai.html).";
        }
        else if (thinkingType == "Unknown") {
            return "Thinking level for model is unknown; none is selected by default.";
        }
        else {
            return "";
        }
    }
    function getApplicableThinkingOptions(thinkingType) {
        if (thinkingType === "ThinkingLevels") {
            return [
                {
                    id: "low",
                    description: "Low",
                    additionalInfo: "Thinks less, quicker"
                },
                {
                    id: "high",
                    description: "High",
                    additionalInfo: "Thinks more, slower"
                }
            ];
        }
        else if (thinkingType === "Unknown") {
            return [
                {
                    id: "none",
                    description: "None",
                    additionalInfo: "No thinking/reasoning"
                },
                {
                    id: "low",
                    description: "Low",
                    additionalInfo: "Thinks less, quicker"
                },
                {
                    id: "high",
                    description: "High",
                    additionalInfo: "Thinks more, slower"
                }
            ];
        }
        else {
            return [];
        }
    }
}
async function collectAndPersistAiSettingsFromPromptResult(promptResult, providerModelsResult, aiSettingKeyPrefix) {
    const providerId = providerModelsResult.modelLookup[promptResult.model].providerId;
    const modelId = providerModelsResult.modelLookup[promptResult.model].modelName;
    const thinkingLevel = promptResult.thinking;
    const globalSettings = await userSettings.loadGlobalAsync();
    globalSettings.set(`${aiSettingKeyPrefix}.ModelId`, `${providerId}--${modelId}`);
    globalSettings.set(`${aiSettingKeyPrefix}.ThinkingLevel`, thinkingLevel);
    return { providerId, modelId, thinkingLevel: thinkingLevel };
}
class Icons {
}
Icons.AiAngular = "data:image/svg+xml;base64,PD94bWwgdmVyc2lvbj0iMS4wIiBlbmNvZGluZz0iVVRGLTgiPz4KPHN2ZyBpZD0idXVpZC1jMjk3NGQzMy04MjUzLTQzYjUtYjNjOS01YjFmMWI3MjczNWEiIGRhdGEtbmFtZT0iYnJhY2tldCBzeW1ib2wtYmx1ZSIgeG1sbnM9Imh0dHA6Ly93d3cudzMub3JnLzIwMDAvc3ZnIiB4bWxuczp4bGluaz0iaHR0cDovL3d3dy53My5vcmcvMTk5OS94bGluayIgdmlld0JveD0iMCAwIDQwLjI1IDQxLjA2Ij4KICA8ZGVmcz4KICAgIDxzdHlsZT4KICAgICAgLnV1aWQtN2M0MmM5YWItZmY5OC00OWFjLTgyMWUtMTdkODg0YzU2MGUzIHsKICAgICAgICBmaWxsOiB1cmwoI3V1aWQtNjNiYmM0YjktMjc0Mi00YjYwLWI3ODAtM2M0NTNhODhmODNjKTsKICAgICAgfQoKICAgICAgLnV1aWQtYzU4YjcxOGYtNGY0YS00ZTQzLTk0MTAtNjA1MzYxYzIyMzc3IHsKICAgICAgICBmaWxsOiB1cmwoI3V1aWQtZWFjMDdkODQtNzdkYi00ZGU3LWJjZTYtNDI2OTUwNTdkMTM0KTsKICAgICAgfQoKICAgICAgLnV1aWQtOTc4NmQxZTMtNWRlMC00ZmM5LTgxMTItMmY4Y2YzNzcyZGQ4IHsKICAgICAgICBmaWxsOiB1cmwoI3V1aWQtMTJlYjkyMjEtZTc0NC00NGJiLWFmZGMtNTQwZDNiNTM5YTg4KTsKICAgICAgfQoKICAgICAgLnV1aWQtNzhhY2RhNDQtODMwNS00ZTYwLWEwZGYtMGJjYjUzOGQ1MWI4IHsKICAgICAgICBmaWxsOiAjZmZmOwogICAgICB9CgogICAgICAudXVpZC1lOGY4YjBhNS02Yzc2LTRjNjEtYTJkZi1jYzUxMjM2NzY0YzkgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC1jODY1ZWFkZi1hMjY4LTRkMGMtODRkNS0xYjk1NDhiMjQ5ZWUpOwogICAgICB9CgogICAgICAudXVpZC1kOWY5YzJjNC00NGVmLTRjZmItODE1Ny1jYTUzY2I4NjhhMjkgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC1lYmQ4ZTM1NS04YjFjLTQ2ZDYtYTJmMC0xZDBlOWQ3N2RhMzgpOwogICAgICB9CgogICAgICAudXVpZC05MTg4N2RhMS03ZmI5LTRkYWQtOTMyMC0xMmZiM2Y5NzM0NjQgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC1hNmJkNDU1My0xZjI2LTQyYjEtYmEwMi0zNGVhNTdiOWM1MTcpOwogICAgICB9CgogICAgICAudXVpZC1mMzI4OTc3Mi1iYmE1LTQyYTMtOWM0ZC1iZTk4ZDFjYWE0M2UgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC00MzAxNGIzNi0wNjA2LTQzNjMtYjY1MS1lMjk3NzJmOWI5OTkpOwogICAgICB9CgogICAgICAudXVpZC02NzBmMjEzZi02MTRiLTRjZWItOTM0NS1jMzcyMzliYzMzNjUgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC0wNmFhYTMyMy0wNDA0LTQ3MTItYjgxNC05YWNkY2FhMzlkZjEpOwogICAgICB9CgogICAgICAudXVpZC03YTg0YjU0YS0wZjM0LTQ1NTQtYTI5Mi01ZmIxYTY5NGNmYjkgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC03ODgwNjYzMS1jM2VhLTQ0MDMtODJmOS1iYzUwZDc4MDk5ZWMpOwogICAgICB9CgogICAgICAudXVpZC0xZTY2MDY5OC01NzI3LTQ1ODAtYjgzYS02MTlhNTAxZmI5ZTAgewogICAgICAgIGZpbGw6IHVybCgjdXVpZC0zZjYyZTE2Zi1jNjI5LTQwMzktYTZlZi0wNzg0MDkyMTBiM2YpOwogICAgICB9CiAgICA8L3N0eWxlPgogICAgPGxpbmVhckdyYWRpZW50IGlkPSJ1dWlkLWViZDhlMzU1LThiMWMtNDZkNi1hMmYwLTFkMGU5ZDc3ZGEzOCIgeDE9Ii0xMjYxNC45OSIgeTE9Ijc3OTEuMzYiIHgyPSItMTI2MTQuOTkiIHkyPSI3NzczLjA2IiBncmFkaWVudFRyYW5zZm9ybT0idHJhbnNsYXRlKC03NzcxIC0xMjU4OC44Nykgcm90YXRlKC05MCkiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj4KICAgICAgPHN0b3Agb2Zmc2V0PSIwIiBzdG9wLWNvbG9yPSIjZmZmIi8+CiAgICAgIDxzdG9wIG9mZnNldD0iLjQiIHN0b3AtY29sb3I9IiMwOWM0ZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIxIiBzdG9wLWNvbG9yPSIjMDA3MGMwIi8+CiAgICA8L2xpbmVhckdyYWRpZW50PgogICAgPGxpbmVhckdyYWRpZW50IGlkPSJ1dWlkLTNmNjJlMTZmLWM2MjktNDAzOS1hNmVmLTA3ODQwOTIxMGIzZiIgeDE9Ii0xNzMxMy4wNiIgeTE9IjIyODMuNiIgeDI9Ii0xNzMxMy4wNiIgeTI9IjIyNjUuNSIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtMTM4MzEuNyAtMTA2MDQuNzUpIHJvdGF0ZSgtMTM1KSIgZ3JhZGllbnRVbml0cz0idXNlclNwYWNlT25Vc2UiPgogICAgICA8c3RvcCBvZmZzZXQ9IjAiIHN0b3AtY29sb3I9IiNmZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuNCIgc3RvcC1jb2xvcj0iIzA5YzRmZiIvPgogICAgICA8c3RvcCBvZmZzZXQ9IjEiIHN0b3AtY29sb3I9IiMwMDcwYzAiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgICA8bGluZWFyR3JhZGllbnQgaWQ9InV1aWQtMDZhYWEzMjMtMDQwNC00NzEyLWI4MTQtOWFjZGNhYTM5ZGYxIiB4MT0iLTE2NzM5Ljk3IiB5MT0iLTQ5MzIuNzQiIHgyPSItMTY3MzkuOTciIHkyPSItNDk1MC45NCIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtMTY3MTMuODcgLTQ5MTUuODcpIHJvdGF0ZSgtMTgwKSIgZ3JhZGllbnRVbml0cz0idXNlclNwYWNlT25Vc2UiPgogICAgICA8c3RvcCBvZmZzZXQ9IjAiIHN0b3AtY29sb3I9IiNmZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuNCIgc3RvcC1jb2xvcj0iIzA5YzRmZiIvPgogICAgICA8c3RvcCBvZmZzZXQ9IjEiIHN0b3AtY29sb3I9IiMwMDcwYzAiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgICA8bGluZWFyR3JhZGllbnQgaWQ9InV1aWQtNjNiYmM0YjktMjc0Mi00YjYwLWI3ODAtM2M0NTNhODhmODNjIiB4MT0iLTExMjMyLjU4IiB5MT0iLTk2MzAuMjkiIHgyPSItMTEyMzIuNTgiIHkyPSItOTY0OC40OSIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtMTQ3MjkuNzUgMTE0NC45Mykgcm90YXRlKDEzNSkiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj4KICAgICAgPHN0b3Agb2Zmc2V0PSIwIiBzdG9wLWNvbG9yPSIjZmZmIi8+CiAgICAgIDxzdG9wIG9mZnNldD0iLjQiIHN0b3AtY29sb3I9IiMwOWM0ZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIxIiBzdG9wLWNvbG9yPSIjMDA3MGMwIi8+CiAgICA8L2xpbmVhckdyYWRpZW50PgogICAgPGxpbmVhckdyYWRpZW50IGlkPSJ1dWlkLTQzMDE0YjM2LTA2MDYtNDM2My1iNjUxLWUyOTc3MmY5Yjk5OSIgeDE9Ii00MDE1Ljg3IiB5MT0iLTkwNTcuNzQiIHgyPSItNDAxNS44NyIgeTI9Ii05MDc1Ljk0IiBncmFkaWVudFRyYW5zZm9ybT0idHJhbnNsYXRlKC05MDQwLjc3IDQwMjcuMSkgcm90YXRlKDkwKSIgZ3JhZGllbnRVbml0cz0idXNlclNwYWNlT25Vc2UiPgogICAgICA8c3RvcCBvZmZzZXQ9IjAiIHN0b3AtY29sb3I9IiNmZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuNCIgc3RvcC1jb2xvcj0iIzA5YzRmZiIvPgogICAgICA8c3RvcCBvZmZzZXQ9IjEiIHN0b3AtY29sb3I9IiMwMDcwYzAiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgICA8bGluZWFyR3JhZGllbnQgaWQ9InV1aWQtNzg4MDY2MzEtYzNlYS00NDAzLTgyZjktYmM1MGQ3ODA5OWVjIiB4MT0iNjgxLjQ2IiB5MT0iLTM1NTAuMDciIHgyPSI2ODEuNDYiIHkyPSItMzU2OC4yNyIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtMjk4MC4wNyAyMDQyLjg4KSByb3RhdGUoNDUpIiBncmFkaWVudFVuaXRzPSJ1c2VyU3BhY2VPblVzZSI+CiAgICAgIDxzdG9wIG9mZnNldD0iMCIgc3RvcC1jb2xvcj0iI2ZmZiIvPgogICAgICA8c3RvcCBvZmZzZXQ9Ii40IiBzdG9wLWNvbG9yPSIjMDljNGZmIi8+CiAgICAgIDxzdG9wIG9mZnNldD0iMSIgc3RvcC1jb2xvcj0iIzAwNzBjMCIvPgogICAgPC9saW5lYXJHcmFkaWVudD4KICAgIDxsaW5lYXJHcmFkaWVudCBpZD0idXVpZC1hNmJkNDU1My0xZjI2LTQyYjEtYmEwMi0zNGVhNTdiOWM1MTciIHgxPSIxMDkuMTMiIHkxPSIzNjY2LjM2IiB4Mj0iMTA5LjEzIiB5Mj0iMzY0OC4wNiIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtOTcuOSAtMzY0NikiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj4KICAgICAgPHN0b3Agb2Zmc2V0PSIwIiBzdG9wLWNvbG9yPSIjZmZmIi8+CiAgICAgIDxzdG9wIG9mZnNldD0iLjQiIHN0b3AtY29sb3I9IiMwOWM0ZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIxIiBzdG9wLWNvbG9yPSIjMDA3MGMwIi8+CiAgICA8L2xpbmVhckdyYWRpZW50PgogICAgPGxpbmVhckdyYWRpZW50IGlkPSJ1dWlkLTEyZWI5MjIxLWU3NDQtNDRiYi1hZmRjLTU0MGQzYjUzOWE4OCIgeDE9Ii01Mzk5LjAyIiB5MT0iODM2My44MiIgeDI9Ii01Mzk5LjAyIiB5Mj0iODM0NS43MiIgZ3JhZGllbnRUcmFuc2Zvcm09InRyYW5zbGF0ZSgtMjA4Mi4xMiAtOTcwNi43KSByb3RhdGUoLTQ1KSIgZ3JhZGllbnRVbml0cz0idXNlclNwYWNlT25Vc2UiPgogICAgICA8c3RvcCBvZmZzZXQ9IjAiIHN0b3AtY29sb3I9IiNmZmYiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuNCIgc3RvcC1jb2xvcj0iIzA5YzRmZiIvPgogICAgICA8c3RvcCBvZmZzZXQ9IjEiIHN0b3AtY29sb3I9IiMwMDcwYzAiLz4KICAgIDwvbGluZWFyR3JhZGllbnQ+CiAgICA8bGluZWFyR3JhZGllbnQgaWQ9InV1aWQtZWFjMDdkODQtNzdkYi00ZGU3LWJjZTYtNDI2OTUwNTdkMTM0IiB4MT0iMjMuNDYiIHkxPSI3LjYiIHgyPSIzNy40IiB5Mj0iMTQuMjMiIGdyYWRpZW50VHJhbnNmb3JtPSJ0cmFuc2xhdGUoMCA0MS4zMykgc2NhbGUoMSAtMSkiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj4KICAgICAgPHN0b3Agb2Zmc2V0PSIwIiBzdG9wLWNvbG9yPSIjZTQwMDM1Ii8+CiAgICAgIDxzdG9wIG9mZnNldD0iLjIiIHN0b3AtY29sb3I9IiNmNjBhNDgiLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIuNCIgc3RvcC1jb2xvcj0iI2YyMDc1NSIvPgogICAgICA8c3RvcCBvZmZzZXQ9Ii41IiBzdG9wLWNvbG9yPSIjZGMwODdkIi8+CiAgICAgIDxzdG9wIG9mZnNldD0iLjciIHN0b3AtY29sb3I9IiM5NzE3ZTciLz4KICAgICAgPHN0b3Agb2Zmc2V0PSIxIiBzdG9wLWNvbG9yPSIjNmMwMGY1Ii8+CiAgICA8L2xpbmVhckdyYWRpZW50PgogICAgPGxpbmVhckdyYWRpZW50IGlkPSJ1dWlkLWM4NjVlYWRmLWEyNjgtNGQwYy04NGQ1LTFiOTU0OGIyNDllZSIgeDE9IjI1LjQ5IiB5MT0iMTcuODgiIHgyPSIzNC42MSIgeTI9IjcuNDciIGdyYWRpZW50VHJhbnNmb3JtPSJ0cmFuc2xhdGUoMCA0MS4zMykgc2NhbGUoMSAtMSkiIGdyYWRpZW50VW5pdHM9InVzZXJTcGFjZU9uVXNlIj4KICAgICAgPHN0b3Agb2Zmc2V0PSIwIiBzdG9wLWNvbG9yPSIjZmYzMWQ5Ii8+CiAgICAgIDxzdG9wIG9mZnNldD0iMSIgc3RvcC1jb2xvcj0iI2ZmNWJlMSIgc3RvcC1vcGFjaXR5PSIwIi8+CiAgICA8L2xpbmVhckdyYWRpZW50PgogIDwvZGVmcz4KICA8cGF0aCBjbGFzcz0idXVpZC1kOWY5YzJjNC00NGVmLTRjZmItODE1Ny1jYTUzY2I4NjhhMjkiIGQ9Ik0yMC4zNiwyMC4zNmMwLS40LjQsMy43LTEuNSw2LjgtMS44LDMuMS00LjEsNC03LjcsNC44LTIsLjUtMy4yLDAtNC44LS41LTIuMi0uNy0zLjMtMi41LTMuNy0zLjFzLS44LTEuOS0uNS0zLjJjLjUtMi4yLDItMy4zLDQtMy45LjQsMCwyLjItLjUsMy45LjRzMi4yLDEuNSwzLjcsMS45YzEuNS40LDMuMSwwLDMuNy0uNCwxLjctLjYsMi45LTIuNCwzLjEtMy4xdi4yaC0uMnYuMVoiLz4KICA8cGF0aCBjbGFzcz0idXVpZC0xZTY2MDY5OC01NzI3LTQ1ODAtYjgzYS02MTlhNTAxZmI5ZTAiIGQ9Ik0yMS4wNiwxOC41NmMtLjMtLjMsMi45LDIuNCwzLjcsNS44cy4yLDUuOC0yLDguOWMtMS4yLDEuOC0yLjMsMi40LTMuOCwzLjItMi4xLDEtNC4yLjYtNC43LjVzLTEuOS0uNy0yLjYtMS45Yy0xLjEtMS45LTEtMy44LDAtNS42LjMtLjMsMS4xLTEuOSwzLTIuNSwxLjgtLjYsMi42LS41LDQtMS4zczIuMS0yLjEsMi40LTIuOWMuNy0xLjcuNC0zLjcsMC00LjNoLjItLjJ2LjFaIi8+CiAgPHBhdGggY2xhc3M9InV1aWQtNjcwZjIxM2YtNjE0Yi00Y2ViLTkzNDUtYzM3MjM5YmMzMzY1IiBkPSJNMjAuMzYsMTYuOTZjLS40LDAsMy43LS40LDYuOCwxLjUsMy4xLDEuOCw0LjMsNCw0LjgsNy43LjQsMiwwLDMuMi0uNSw0LjgtLjcsMi4yLTIuNSwzLjMtMy4xLDMuN3MtMS45LjgtMy4yLjVjLTIuMi0uNS0zLjMtMi0zLjktNCwwLS40LS41LTIuMi40LTMuOXMxLjUtMi4yLDEuOS0zLjcsMC0zLjEtLjQtMy43Yy0uNi0xLjctMi40LTIuOS0zLjEtMy4xaC4ydi4yaC4xWiIvPgogIDxwYXRoIGNsYXNzPSJ1dWlkLTdjNDJjOWFiLWZmOTgtNDlhYy04MjFlLTE3ZDg4NGM1NjBlMyIgZD0iTTE4LjU2LDE2LjI2Yy0uMy4zLDIuNC0yLjksNS44LTMuN3M1LjgtLjIsOC45LDJjMS44LDEuMiwyLjQsMi4zLDMuMiwzLjgsMSwyLjEuNiw0LjIuNSw0LjdzLS43LDEuOS0xLjksMi42Yy0xLjksMS4xLTMuOCwxLTUuNiwwLS4zLS4zLTEuOS0xLjEtMi41LTNzLS41LTIuNi0xLjMtNC0yLjEtMi4xLTIuOS0yLjRjLTEuNy0uNy0zLjctLjQtNC4zLDBoMHYtLjJoMHYuMnMuMSwwLC4xLDBaIi8+CiAgPHBhdGggY2xhc3M9InV1aWQtZjMyODk3NzItYmJhNS00MmEzLTljNGQtYmU5OGQxY2FhNDNlIiBkPSJNMTYuOTYsMTYuOTZjMCwuNC0uNC0zLjcsMS41LTYuOHM0LTQuMyw3LjctNC44YzItLjQsMy4yLDAsNC44LjUsMi4yLjcsMy4zLDIuNSwzLjcsMy4xcy44LDEuOS41LDMuMmMtLjUsMi4yLTIsMy4zLTQsMy45LS40LDAtMi4yLjUtMy45LS40cy0yLjItMS41LTMuNy0xLjktMy4xLDAtMy43LjRjLTEuNy42LTIuOSwyLjQtMy4xLDMuMXYtLjJoLjJ2LS4xWiIvPgogIDxwYXRoIGNsYXNzPSJ1dWlkLTdhODRiNTRhLTBmMzQtNDU1NC1hMjkyLTVmYjFhNjk0Y2ZiOSIgZD0iTTE2LjI2LDE4LjU2Yy4zLjMtMi45LTIuNC0zLjctNS44cy0uMi01LjgsMi04LjljMS4yLTEuOCwyLjMtMi40LDMuOC0zLjIsMi4xLTEsNC4yLS42LDQuNy0uNXMxLjkuNywyLjYsMS45YzEuMSwxLjksMSwzLjgsMCw1LjYtLjMuMy0xLjEsMS45LTMsMi41cy0yLjYuNS00LDEuM2MtMS4zLjgtMi4xLDIuMS0yLjQsMi45LS43LDEuNy0uNCwzLjcsMCw0LjNoLS4yLjJ2LS4xWiIvPgogIDxwYXRoIGNsYXNzPSJ1dWlkLTkxODg3ZGExLTdmYjktNGRhZC05MzIwLTEyZmIzZjk3MzQ2NCIgZD0iTTE2Ljk2LDIwLjM2Yy40LDAtMy43LjQtNi44LTEuNXMtNC4zLTQtNC44LTcuN2MtLjQtMiwwLTMuMi41LTQuOC43LTIuMiwyLjUtMy4zLDMuMS0zLjdzMS45LS44LDMuMi0uNWMyLjIuNSwzLjMsMiwzLjksNCwwLC40LjUsMi4yLS40LDMuOXMtMS41LDIuMi0xLjksMy43Yy0uNCwxLjUsMCwzLjEuNCwzLjcuNiwxLjcsMi40LDIuOSwzLjEsMy4xaC0uMnYtLjJoLS4xWiIvPgogIDxwYXRoIGNsYXNzPSJ1dWlkLTk3ODZkMWUzLTVkZTAtNGZjOS04MTEyLTJmOGNmMzc3MmRkOCIgZD0iTTE4LjU2LDIxLjA2Yy4zLS4zLTIuNCwyLjktNS44LDMuN3MtNS44LjItOC45LTJjLTEuOC0xLjItMi40LTIuMy0zLjItMy44Qy0uMzQsMTYuODYuMDYsMTQuNzYuMTYsMTQuMjZzLjctMS45LDEuOS0yLjZjMS45LTEuMSwzLjgtMSw1LjYsMCwuMy4zLDEuOSwxLjEsMi41LDMsLjYsMS44LjUsMi42LDEuMyw0LC44LDEuMywyLjEsMi4xLDIuOSwyLjQsMS43LjcsMy43LjQsNC4zLDBoMHYuMmgwdi0uMnMtLjEsMC0uMSwwWiIvPgogIDxwb2x5Z29uIGNsYXNzPSJ1dWlkLTc4YWNkYTQ0LTgzMDUtNGU2MC1hMGRmLTBiY2I1MzhkNTFiOCIgcG9pbnRzPSIzOS41NiAzNC45NyAzNS42MyAzOC4xNyAyNS42MSAzOC4xNyAyMS42OCAzNC45NyAyOC4zMyAyMC42MiAzMi45IDIwLjYyIDM5LjU2IDM0Ljk3Ii8+CiAgPHBhdGggY2xhc3M9InV1aWQtNzhhY2RhNDQtODMwNS00ZTYwLWEwZGYtMGJjYjUzOGQ1MWI4IiBkPSJNNDAuMjUsMjQuMDdsLS43MiwxMC45LTYuNjUtMTQuMzUsNy4zNywzLjQ1Wk0zNS42LDM4LjE4bC00Ljk3LDIuODktNS4wNS0yLjg5Ljk2LTIuNDhoOC4xbC45NiwyLjQ4Wk0zMC42MywyNi4wOGwyLjU2LDYuNDFoLTUuMjFsMi42NC02LjQxWk0yMS42NSwzNC45N2wtLjcyLTEwLjksNy4zNy0zLjQ1LTYuNjUsMTQuMzVaIi8+CiAgPHBhdGggY2xhc3M9InV1aWQtNzhhY2RhNDQtODMwNS00ZTYwLWEwZGYtMGJjYjUzOGQ1MWI4IiBkPSJNMzAuNjMsMjYuMDhsMi41Niw2LjQxaC01LjIxbDIuNjQtNi40MVoiLz4KICA8cG9seWdvbiBjbGFzcz0idXVpZC03OGFjZGE0NC04MzA1LTRlNjAtYTBkZi0wYmNiNTM4ZDUxYjgiIHBvaW50cz0iMzQuNjMgMzUuNjkgMzUuNTkgMzguMTcgMzAuNjIgNDEuMDYgMjUuNTggMzguMTcgMjYuNTQgMzUuNjkgMzQuNjMgMzUuNjkiLz4KICA8cG9seWdvbiBjbGFzcz0idXVpZC03OGFjZGE0NC04MzA1LTRlNjAtYTBkZi0wYmNiNTM4ZDUxYjgiIHBvaW50cz0iMjguNTIgMjAuNjIgMjEuODcgMzQuOTcgMjEuMTUgMjQuMDcgMjguNTIgMjAuNjIiLz4KICA8cG9seWdvbiBjbGFzcz0idXVpZC03OGFjZGE0NC04MzA1LTRlNjAtYTBkZi0wYmNiNTM4ZDUxYjgiIHBvaW50cz0iNDAuMjQgMjQuMDcgMzkuNTIgMzQuOTcgMzIuODcgMjAuNjIgNDAuMjQgMjQuMDciLz4KICA8cGF0aCBjbGFzcz0idXVpZC1jNThiNzE4Zi00ZjRhLTRlNDMtOTQxMC02MDUzNjFjMjIzNzciIGQ9Ik0zOS4zNSwyNC43MWwtLjY1LDkuODgtNi4wMy0xMy4wMSw2LjY5LDMuMTJaTTM1LjEzLDM3LjVsLTQuNTEsMi42Mi00LjU4LTIuNjIuODctMi4yNWg3LjM0bC44NywyLjI1Wk0zMC42MywyNi41MmwyLjMzLDUuODFoLTQuNzJsMi40LTUuODFaTTIyLjQ5LDM0LjU5bC0uNjUtOS44OCw2LjY5LTMuMTItNi4wMywxMy4wMVoiLz4KICA8cGF0aCBjbGFzcz0idXVpZC1lOGY4YjBhNS02Yzc2LTRjNjEtYTJkZi1jYzUxMjM2NzY0YzkiIGQ9Ik0zOS4zNSwyNC43MWwtLjY1LDkuODgtNi4wMy0xMy4wMSw2LjY5LDMuMTJaTTM1LjEzLDM3LjVsLTQuNTEsMi42Mi00LjU4LTIuNjIuODctMi4yNWg3LjM0bC44NywyLjI1Wk0zMC42MywyNi41MmwyLjMzLDUuODFoLTQuNzJsMi40LTUuODFaTTIyLjQ5LDM0LjU5bC0uNjUtOS44OCw2LjY5LTMuMTItNi4wMywxMy4wMVoiLz4KPC9zdmc+";
