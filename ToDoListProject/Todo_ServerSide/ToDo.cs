﻿using System;
using System.Collections.Generic;

namespace TodoApi;

public partial class ToDo
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public sbyte? IsComplete { get; set; }
}
