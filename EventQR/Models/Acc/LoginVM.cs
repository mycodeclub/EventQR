﻿namespace EventQR.Models.Acc
{
    public class LoginVM
    {
        public string LoginName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public bool RememberMe { get; set; }
    }
}
