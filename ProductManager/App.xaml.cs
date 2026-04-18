namespace ProductManager
{
    // app receives AppShell from DI container.
    public partial class App : Application
    {
        private readonly AppShell _appShell;

        public App(AppShell appShell)
        {
            InitializeComponent();
            _appShell = appShell;

        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            // we use injected AppShell instead of creating it manually.
            return new Window(_appShell);
        }
    }
}