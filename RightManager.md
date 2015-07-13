# Introduction #
This module allows the user that is granted to utilise this plugin to administrate the rights of the application.

The role manager is divided into two parts: the roles and the tasks.

A **task** is an action the connected user can perform or not depending on the assigned **role** he/she has. nDoctor has a built-in list of tasks that **cannot** be modified.

A **role** is a batch of tasks. A user has an assigned role that allows him/her to execute certain actions. Only administrators can create roles.

# Tasks list #
| **Id**             | **Name**                                   |
|:-------------------|:-------------------------------------------|
| Administer         | Regroup all the features that can read/write data related to the application management (such as authorisation or data import)            |
| Meta\_write        | Regroup all the features that are about the connected user           |
| Write              | Regroup all the features that can modify the data              |
| Read               | Regroup all the features that read data              |
| Edit\_Calendar     | Regroup all the features that can modify the data of the calendar                  |

# Built-in role list #
| **Id**             | **Name**                                   |
|:-------------------|:-------------------------------------------|
| Administrator      | this user has got all the privileges in the application            |
| Doctor             | this user can read and write all the data but the authorisation or any administration pieces of data       |
| Secretary          | this user can read all the data but can only modify data related to calendar and appointment management.               |

# List of features granted to everyone #

| **Component**              | **Granted action**                            |
|:---------------------------|:----------------------------------------------|
| Administration Component   | Creation of a new tag                         |
| Picture Component          | Creation of all the thumbnails                |
| SQL Component              | Find patients by name and/od surname          |
| User Session Component     | Check if a patient can connect into nDoctor   |
| Selector                   | Create a new user with a password             |