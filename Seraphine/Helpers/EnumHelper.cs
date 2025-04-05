using System;

namespace Seraphine.Helpers
{
    public static class EnumHelper
    {
        public static T Converter<T>(string tipo) where T : struct, Enum
        {
            var tipoEnum = typeof(T);
            if (Enum.TryParse(tipoEnum, tipo, true, out var eeee))
            {
                return (T)eeee;
            }

            throw new ArgumentException($"Valor '{tipo}' inválido para o enum {typeof(T).Name}");
        }
    }
}
