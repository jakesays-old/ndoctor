# Introduction #

A medical record is a list of text document. Each document has a type.

# GUI #

![https://docs.google.com/drawings/pub?id=1pydNaRvjOFSUEPOmvy7QS5cLBWJFRysMJb5ORMRo6zQ&w=640&h=480&nonsense=something_that_ends_with.png](https://docs.google.com/drawings/pub?id=1pydNaRvjOFSUEPOmvy7QS5cLBWJFRysMJb5ORMRo6zQ&w=640&h=480&nonsense=something_that_ends_with.png)

# Features #
  * The medical record manager is a light weight text editor.
  * A tree view shows the hierarchy of the record
  * ~~A search box exist to find a record by name or by type~~ [`Maybe it is a useless feature`]
  * The tool box has all the feature to modify the text appearance such as colour, size, bold or italic.
  * ~~An button exist to rollback the text to the sate it was when loaded.~~ [`This feature will be changed with the autosave`]

## Macro Editor ##

A macro is a text that contains tags that will be replaced with values from the database depending the context.

**The macro edition and management is granted to administrator only**

### Gui ###

The user interface is a dialogue box with:
  * buttons to:
    * Add a new macro.
    * Saving **and** closing the window,
    * Cancel that has the default Windows behaviour.
  * contextual menu on the tree view to:
    * rename the selected macro,
    * delete the selected macro.

The embeded text editor (which is _AvalonEdit_) provides syntax colouration and _IntelliSense_ to help user making macros.

Here's a sketch of the dialogue box:

![https://docs.google.com/drawings/pub?id=1-x_M4TK4neMSi2TyzShwdoO8p6oAcnziwv-ixkU9f6M&w=640&h=480&nonsense=something_that_ends_with.png](https://docs.google.com/drawings/pub?id=1-x_M4TK4neMSi2TyzShwdoO8p6oAcnziwv-ixkU9f6M&w=640&h=480&nonsense=something_that_ends_with.png)

### Keywords ###

Here's the built-in list of keywords (Bear in mind they can't be modified):

| **Keywords**  | **Explanations**|
|:--------------|:----------------|
| $age$         | Displays the age of the patient as a number.|
| $today$       | Displays the today date (without time) in the current culture.|
| $now$         | Displays the today data **with** the current time (hh:mm).|
| $height$      | Displays the height of the patient.|
| $firstname$   | Displays the first name of the patient.|
| $lastname$    | Displays the last name of the patient.|
| $birthdate$   | Displays the birth date if the patient in the current culture|

### Example of usage ###
Here's an example:
> Hello **$firstname$ $lastname$**. Today, **$today$**, you are **$age$** years old.

Which will be result as this with the session of the patient John Doe
> Hello **John Doe**. Today, **2012-12-21**, you are **38** years old

## Auto Save ##

The application will automatically save the current medical record every 30 seconds. The time span could be configured but this feature depends on the **plugin configuration engine** that is not yet implemented.

The last 10 auto-saved states are stacked into a _redo/undo stack_ that allows to roll-back to one of the last auto-saved state.