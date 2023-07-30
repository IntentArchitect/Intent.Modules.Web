//@IntentCanAdd()
export const environment = {auth: {
    authority: 'https://localhost:sts_port',
    client_id: 'Auth_Code_Client',
    redirect_uri: window.location.origin + '/authentication/login-callback',
    post_logout_redirect_uri: window.location.origin + '/authentication/logout-callback',
    response_type: 'code',
    scope: 'openid profile email api roles'
  },
  api_base_url: "https://localhost:{api_port}"
};
