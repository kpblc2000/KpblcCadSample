namespace KpblcCadLoader.Infrastrucute.Enums
{
    /// <summary> Типы приложений, которые будут загружаться в ACAD </summary>
    public enum ApplicationTypeEnum
    {
        /// <summary> Неизвестный тип приложения </summary>
        Unknown,
        /// <summary> NET-сборки </summary>
        Managed,
        /// <summary> arx-модули </summary>
        Arx,
        /// <summary> lsp-модули </summary>
        Lsp,
    }
}
