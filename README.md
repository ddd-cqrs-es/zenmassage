# Zen Massage

Three applications designed to explore three different runtime environments.

## Zen Massage Site

Seed web application project using;
 
 * Angular 2
 * Bootstrap 4
 * Typescript
 * SystemJS
 * MVC 6
 * NPM
 * Gulp

## Zen Massage (Mobile)

Companion Apache Cordova mobile project using;

 * Cordova 6.0.0
 * Onsen UI 2.0.0 beta
 * Angular 1
 * Typescript
 * NPM
 * Gulp

## Zen Massage (Pebble)

Pebble smart-watch application (via different github repo github.com/dementeddevil/zenmassagepebble )
This application makes use of the seed web application to host the Pebble App Configuration Pages and
will eventually explore management of shared and user-specific timeline pins.
This application will also feature tight integration with the mobile application (at least on Android)

Getting all these moving parts to play nicely together is fairly tricky and subject to change since I'm riding the beta wave on not one but two different frameworks!

Feel free to contribute, comment and reuse all that you see here.

# Requirements
1. An internet connection (so you can download node modules and such)
2. Visual Studio 2015
   You will need the Azure SDK (v2.8.2 or higher) and Apache Cordova Tools for Visual Studio (Update 6 or higher)
3. NodeJS (currently using version 5.5.0)

# Application Structure

The web-site is a multi-tier web application based on MVC6. It follows a CQRS (Command Query Response Seperation) pattern that keeps the read side (query) and write side (command) distinct.

The write side is supported by a Domain-Driven-Design persistence model where changes are written to a transaction log and published to a message bus.
The read side is supported by a typical code-first Entity Framework database.
The read and write sides are kept in sync by an update handler that listens for events on the message bus and plays the corresponding actions into the read side database.
This means that the system employs eventual consistency.

The site also exposes a set of WEB APIs that allow access to underlying services. These services are documented by Swagger (/swagger/ui)

# Next Steps
1. Integration of unit testing of both server-side and client-side code (likely using xUnit and mocha)
2. Fleshing out Angular logic and adding some nice bells and whistles
3. Making the Bootstrap UI look a little nicer

# Credits
Big thanks to Dan Wahlin for the Angular 2 starting point.
