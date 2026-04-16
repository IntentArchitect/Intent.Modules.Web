const navigationSourceEndSpecializationId = "97a3de8a-c9bf-4cf2-bc0a-b8692b02211b";
const navigationTargetEndSpecializationId = "2b191288-ecae-4743-b069-cbdd927ef349";
const layoutSpecializationId = "8beaa629-d615-4062-b936-7106530cdf52";

async function createLayoutImplementAICodingTask() {
    let filePaths = element.getAssociatedFiles().map(x => x.absolutePath)

    // build up the context
    let context = '';
    element.getAssociations().filter(a => a.specializationId == navigationTargetEndSpecializationId).forEach(nav => {
        let appendComment = `- A menu item MUST be added to navigation to the page "${nav.getName()}"`

        if (context == '') {
            context = appendComment;
        } else {
            context = `${context}\n${appendComment}`;
        }
        
    });

    createAICodingTask({
        title: `Implement Angular Layout: ${element.getName()}`,
        instructions: `Implement "${element.getName()}" using the appropriate skill.`,
        context: context,
        filesToInclude: filePaths
    });
}

async function createComponentImplementAICodingTask() {
    let filePaths = element.getAssociatedFiles().map(x => x.absolutePath)
    let menuNavigations = element.getAssociations().filter(a => a.specializationId == navigationSourceEndSpecializationId
        && a.typeReference?.typeId == layoutSpecializationId);

    let navContext = '';
    let navInstruction = '';
    if (menuNavigations.length > 0) {
        const layoutElement = lookup(menuNavigations[0].getParent()?.id);
        if (layoutElement) {
            filePaths = filePaths.concat(layoutElement.getAssociatedFiles().map(x => x.absolutePath))

            // build up the context
            let navContext = '';
            element.getAssociations().filter(a => a.specializationId == "2b191288-ecae-4743-b069-cbdd927ef349").forEach(nav => {
                let appendComment = `- A menu item MUST be added to navigation to the page "${nav.getName()}"`
                if (navContext == '') {
                    navContext = appendComment;
                } else {
                    navContext = `${navContext}\n${appendComment}`;
                }
            });

            navInstruction = `and "${layoutElement.getName()}" `
        }
    }

    createAICodingTask({
        title: `Implement Angular Component: ${element.getName()}`,
        instructions: `Implement "${element.getName()}" ${navInstruction}using the appropriate skill(s).`,
        context: navContext,
        filesToInclude: filePaths
    });
}
