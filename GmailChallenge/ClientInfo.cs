using Newtonsoft.Json.Linq;
using System;
using System.IO;

namespace Gmail_Challenge
{
    /// <summary>
    /// OAuth 2.0 client ID used by this application loaded from a client ID json file.
    /// Set the TEST_WEB_CLIENT_SECRET_FILENAME environment variable to point to the client ID json file.
    /// </summary>
    public class ClientInfo
    {
        private const string ClientSecretFilenameVariable = "gmail-challenge-1612295020869";

        public static ClientInfo Load()
        {
            string clientSecretFilename = $".{Path.DirectorySeparatorChar}Resources{Path.DirectorySeparatorChar}client_secret.json";//Environment.GetEnvironmentVariable(ClientSecretFilenameVariable);
            if (string.IsNullOrEmpty(clientSecretFilename))
            {
                throw new InvalidOperationException($"Please set the {ClientSecretFilenameVariable} environment variable before running tests.");
            }

            var secrets = JObject.Parse(File.ReadAllText(clientSecretFilename))["web"];

            // Check that this is a "web" client ID, not any other type of client ID like "installed app".
            // The "web" element should have been present in the json so secrets value shouldn't be null.
            if (secrets is null)
            {
                throw new InvalidOperationException(
                    $"The type of the OAuth2 client ID specified in {ClientSecretFilenameVariable} should be Web Application. You can read more about setting up OAuth2 client IDs here: https://support.google.com/cloud/answer/6158849?hl=en");
            }
            var projectId = secrets["project_id"].Value<string>();
            var clientId = secrets["client_id"].Value<string>();
            var clientSecret = secrets["client_secret"].Value<string>();

            return new ClientInfo(projectId, clientId, clientSecret);
        }

        private ClientInfo(string projectId, string clientId, string clientSecret)
        {
            ProjectId = projectId;
            ClientId = clientId;
            ClientSecret = clientSecret;
        }

        public string ProjectId { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }
    }
}
