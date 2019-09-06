# The `FROM` instruction specifies the base image. You are
# extending the `microsoft/aspnet` image.

FROM microsoft/aspnet

# Next, this Dockerfile creates a directory for your application
RUN mkdir C:\app

# configure the new site in IIS.
RUN powershell -NoProfile -Command \
    Import-module IISAdministration; \
    New-IISSite -Name "ASPNET" -PhysicalPath C:\app -BindingInformation "*:44365:"

# This instruction tells the container to listen on port 44365. 
EXPOSE 44365

# The final instruction copies the site you published earlier into the container.
ADD . /app