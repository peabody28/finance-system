﻿namespace payment.Interfaces.Entities
{
    public interface ICurrency
    {
        Guid Id { get; set; }

        string Code { get; set; }
    }
}