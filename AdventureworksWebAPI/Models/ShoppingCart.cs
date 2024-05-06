using System;
using System.Collections.Generic;

namespace AdventureWorksWebAPI.Models;

/// <summary>
/// Contains online customer orders until the order is submitted or cancelled.
/// </summary>
public partial class ShoppingCart
{
    /// <summary>
    /// Shopping cart identification number.
    /// </summary>
    public string ShoppingCartId { get; set; } = null!;

   public DateTime? DateCreated { get; set; }
}
