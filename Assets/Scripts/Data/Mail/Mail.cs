using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mail
{
    public string Sender { get; set; }
    public string Subject { get; set; }
    public bool HasRead { get; set; }
    public string Contents { get; set; }
}
