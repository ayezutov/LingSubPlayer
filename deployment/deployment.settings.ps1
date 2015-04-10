$settings = @{
    channels = @{
        "dev" = @{
            "url" = "releases/dev"
        }

        #"qa" = @{
        #    "version" = "0.1.0.0"
        #    "url" = "releases/qa"
        #}

        #"beta" = @{
        #    "version" = "0.1.0.0"
        #    "url" = "releases/beta"
        #}

        "prod" = @{
            "version" = "0.1.0.0"
            "url" = "releases"
        }
    }

    amazon = @{
        bucketName = "lingsubplayer.yezutov.com"
    }
}