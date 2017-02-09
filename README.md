# serilog-enricher-middleware



## Getting started

### Install the package
Install the nuget package from [nuget](https://www.nuget.org/packages/serilog-enricher-middleware/)

Either add it with the PM-Console:
        
        Install-Package serilog-enricher-middleware

Or add it to project.json
        "dependencies": {
            ...
            "serilog-enricher-middleware": "XXX"
        }

### Set your api up

Edit your Startup.cs -> 

        Configure(){
            ...

           tbd.
            
            ...
        }



### Options


## Build and Publish

### Prequisites

* docker, docker-compose
* dotnet core 1.1 sdk  [download core](https://www.microsoft.com/net/core)

The package is build in docker so you will need to install docker to build and publish the package.
(Of course you could just build it on the machine you are running on and publish it from there. 
I prefer to build and publish from docker images to have a reliable environment, plus make it easier 
to build this on circleci).

### build

run:
        docker-compose -f docker-compose-build.yml up

this will build & test the code. The testresult will be in folder ./testresults and the package in ./package

! *if you clone the project on a windows machine you will have to change the file endings on the build-container.sh to LF*

### publish

run: (fill in the api key):

        docker run --rm -v ${PWD}/package:/data/package schwamster/nuget-docker push /data/package/*.nupkg <your nuget api key> -Source nuget.org

this will take the package from ./package and push it to nuget.org

### build on circleci

The project contains a working circle ci yml file. All you have to do is to configure the Nuget Api Key in the build projects environment variables on circleci (Nuget_Api_Key)


