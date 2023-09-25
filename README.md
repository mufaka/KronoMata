# KronoMata
KronoMata is a cross platform scheduled job runner. Scheduled jobs run implementations of IPlugin on configured hosts at configured recurrence intervals. 

## Docker
Docker images are available for the Web application and Agent. They can be run together with the following Docker compose:

```
version: '3.4'

services:
  kronomata.web:
    image: billnickel/kronomata:web
    environment:
      - ASPNETCORE_ENVIRONMENT=Release
    ports:
      - "5015:5002"	  

  kronomata.agent:
    image: billnickel/kronomata:agent 
    environment:
      - KronoMata__APIRoot=http://kronomata.web:5002/api/
```

## Quick Start
KronoMata has 3 main components. The web application for managing scheduled jobs, the agent which is responsible for running the scheduled jobs, and plugins that are the actual executables for scheduled jobs. KronoMata can run on Windows, Linux, or Mac OS as long as .NET 6 is installed.

### Web Application
1. Extract the contents of the KronoMata.Web-<version>.zip.
2. Optionally edit the configuration in appsettings.json. The defaults should be suitable for running immediately.
3. In a console, navigate to the web installation directory and run using 'dotnet KronoMata.Web.dll'
4. Navigate to the URL defined in the appsettings.json 'Urls' configuration value. By default you should be able to access http://localhost:5002/

** You can run the web app as systemd service on Linux, there are some notes in the wiki. On Windows you can use the built in Task Scheduler to create a task that runs on startup. 

### Agent
1. Extract the contents of the KronoMata.Agent-<verson>.zip.
2. Edit the KronoMata:APIRoot value in appsettings.json to point to the web server if you are not running the agent on the same machine as the web server.
3. Be sure that the PackageRoot folder exists. This is where the agent will attempt to store plugins. By default, there needs to be a PackageRoot subdirectory beneath the agent executable.
4. In a console, navigate to the agent installation directory and run using 'dotnet KronoMata.Agent.dll'

The agent will poll the web server every minute for scheduled jobs to run. The first poll will be 1 minute after the agent starts. If this is the first time an agent has polled the web application for scheduled jobs, it will be added as a host that is disabled. Navigate to the Hosts section in the web application and you should see your host added.

![image](https://github.com/mufaka/KronoMata/assets/8632538/d68e3f26-bc57-448a-9529-fd12b243bc4e)

4. Click on the 'More info' link for the host, check the 'Enabled?' checkbox and click the Save button to enable the host.

** You can also run the agent as systemd service on Linux, there are some notes in the wiki for the web app that can be adapted for the agent as well. On Windows you can use the built in Task Scheduler to create a task that runs on startup. 

### Packages
Plugins are implementations of the KronoMata.Public.IPlugin interface. To schedule these plugins you need to upload them to the web server in a zip file. Note that a zip file can contain more than one implementation of IPlugin. The upload process will discover all instances and create the corresponding plugins so that they can be scheduled to run. The release contains a sample plugin that will just echo configured variables to the log.

1. Navigate to the Packages view in the web application.
2. Provide a Package Name on the 'Upload Package' form.
3. Drag and drop, or click to browse for, the KronoMata.SamplePlugin zip file and then click the upload button.
4. If the upload is successful, you will be redirected to the Plugins view. Click on the Echo Plugin to view the parameters that were defined.

![image](https://github.com/mufaka/KronoMata/assets/8632538/8f67fb30-51d7-43dc-87ee-67f4faed3e2d)

### Scheduling
Scheduling jobs can be done for enabled hosts only (or All Hosts but only enabled hosts will execute jobs). 

1. Navigate to the 'Jobs' view and click on the 'Add' button.
2. Fill in the form as shown in the following screen capture.

![image](https://github.com/mufaka/KronoMata/assets/8632538/23489a24-33c7-4fcd-8e1b-272990da58c6)

** Choose the appropriate host and plugin.

3. Click on the 'Save' button.
4. Because the IPlugin implementation provided configuration parameters, you should be redirected to the Scheduled Job Configuration view.
5. Fill in both parameters as the IPlugin implementation defined them as required.

![image](https://github.com/mufaka/KronoMata/assets/8632538/a595cb01-16ac-48ec-aeda-99b55661ed88)

6. Click on the 'Save' button.

If all goes well, you should start to see logs appearing for the execution of the plugins on the agent at the configured schedule. Navigate to the 'History' view to see a complete log.

![image](https://github.com/mufaka/KronoMata/assets/8632538/ffa68891-f5ea-4ff0-b43c-29391b4e6e6a)

Clicking on a history row will provide a popup that shows more information about that execution.

** Note that the first execution in the above screen capture took considerably longer to run. This is because the plugin was downloaded and extracted as part of the first run. Subsequent executions use the agents local version to execute.
