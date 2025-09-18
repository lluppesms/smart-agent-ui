// Copyright (c) Microsoft. All rights reserved.
namespace SmartAgentUI.Models;

public record class UserInformation(bool IsIdentityEnabled, string UserName,string UserId, string SessionId, IEnumerable<ProfileSummary> Profiles, IEnumerable<string> Groups);
