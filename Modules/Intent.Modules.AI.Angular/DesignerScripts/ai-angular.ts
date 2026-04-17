const navigationSourceEndSpecializationId = "97a3de8a-c9bf-4cf2-bc0a-b8692b02211b";
const navigationTargetEndSpecializationId = "2b191288-ecae-4743-b069-cbdd927ef349";
const showDialogTargetEndSpecializationId = "c44a7969-abfa-4073-ab2c-d2d0f1f6bd2f";

const layoutSpecializationId = "8beaa629-d615-4062-b936-7106530cdf52";

async function createLayoutImplementAICodingTask() {
    let filePaths = element.getAssociatedFiles().map(x => x.absolutePath);

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
    let filePaths = element.getAssociatedFiles().map(x => x.absolutePath);

    let intention = '';
    element.getAssociations().filter(a => (a.specializationId == navigationSourceEndSpecializationId ||
        a.specializationId == navigationTargetEndSpecializationId) && a.typeReference.isNavigable).forEach(n => {
            intention += `- This pages navigates to the ${n.getName()} component${"\n"}`;
        });

    element.getChildren("e030c97a-e066-40a7-8188-808c275df3cb").forEach(o => {
        o.getAssociations().filter(a => a.specializationId == showDialogTargetEndSpecializationId).forEach(a => {
            intention += `- The ${o.getName()} operation opens a dialog to show the ${a.typeReference.getType().getName()} component${"\n"}`;
        });
    });

    createAICodingTask({
        title: `Implement Angular Component: ${element.getName()}`,
        instructions: `Implement "${element.getName()}" using the appropriate skill(s).`,
        context: `
            ## User has modeled the following intentions:
            ${intention}`,
        filesToInclude: filePaths
    });
}
