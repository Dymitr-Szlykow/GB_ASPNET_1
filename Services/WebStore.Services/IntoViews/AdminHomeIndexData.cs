using GB.ASPNET.WebStore.Interfaces;

namespace GB.ASPNET.WebStore.Services;

public class AdminHomeIndexData : IVeiwAdminIndexData
{
    private (string href, string FAStyles, string heading)[] items;

    public (string href, string FAStyles, string heading)[] Items
    {
        get => items;
        init
        {
            items = new[]
            {
                ("blank.html", "fa fa-circle-o-notch fa-5x", "Check Data"),
                ("blank.html", "fa fa-envelope-o fa-5x", "Mail Box"),
                ("blank.html", "fa fa-lightbulb-o fa-5x", "New Issues"),
                ("blank.html", "fa fa-users fa-5x", "See Users"),
                ("blank.html", "fa fa-key fa-5x", "Admin"),
                ("blank.html", "fa fa-comments-o fa-5x", "Support"),

                ("blank.html", "fa fa-clipboard fa-5x", "All Docs"),
                ("blank.html", "fa fa-gear fa-5x", "Settings"),
                ("blank.html", "fa fa-wechat fa-5x", "Live Talk"),
                ("blank.html", "fa fa-bell-o fa-5x", "Notifications"),
                ("blank.html", "fa fa-rocket fa-5x", "Launch"),
                ("blank.html", "fa fa-user fa-5x", "Register User"),

                ("blank.html", "fa fa-envelope-o fa-5x", "Mail Box"),
                ("blank.html", "fa fa-lightbulb-o fa-5x", "New Issues"),
                ("blank.html", "fa fa-users fa-5x", "See Users"),
                ("blank.html", "fa fa-key fa-5x", "Admin"),
                ("blank.html", "fa fa-comments-o fa-5x", "Support"),
                ("blank.html", "fa fa-circle-o-notch fa-5x", "Check Data"),

                ("blank.html", "fa fa-rocket fa-5x", "Launch"),
                ("blank.html", "fa fa-clipboard fa-5x", "All Docs"),
                ("blank.html", "fa fa-gear fa-5x", "Settings"),
                ("blank.html", "fa fa-wechat fa-5x", "Live Talk"),
                ("blank.html", "fa fa-bell-o fa-5x", "Notifications"),
                ("blank.html", "fa fa-user fa-5x", "Register User"),

                ("blank.html", "fa fa-lightbulb-o fa-5x", "New Issues"),
                ("blank.html", "fa fa-users fa-5x", "See Users"),
                ("blank.html", "fa fa-key fa-5x", "Admin"),
                ("blank.html", "fa fa-comments-o fa-5x", "Support"),
                ("blank.html", "fa fa-circle-o-notch fa-5x", "Check Data"),
                ("blank.html", "fa fa-envelope-o fa-5x", "Mail Box")
            };
        }    
    }
}