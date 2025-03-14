import SuperTokens from "supertokens-web-js";
import Session from "supertokens-web-js/recipe/session";

export function initSuperTokensUI() {
    (window as any).supertokensUIInit("supertokensui", {
        appInfo: {
            websiteDomain: "http://localhost:3000",
            apiDomain: "http://localhost:3001",
            appName: "SuperTokens Demo App",
        },
        recipeList: [
            (window as any).supertokensUIThirdParty.init({
                signInAndUpFeature: {
                    providers: [
                        (window as any).supertokensUIThirdParty.Github.init(),
                        (window as any).supertokensUIThirdParty.Google.init(),
                        (window as any).supertokensUIThirdParty.Facebook.init(),
                        (window as any).supertokensUIThirdParty.Apple.init(),
                    ]
                }
            }),
            (window as any).supertokensUISession.init(),
        ],
    });
}

export function initSuperTokensWebJS() {
    SuperTokens.init({
        appInfo: {
            appName: "SuperTokens Demo App",
            apiDomain: "http://localhost:3001",
        },
        recipeList: [Session.init()],
    });
}
