#include "AboutWindow.h"

namespace yade
{

class MainMenuBar {
public:
	MainMenuBar(AboutWindow *abWin);
	void drawMenuBar();
protected:
	// TODO: Maybe find a better way of handling this?
	AboutWindow *aboutWindow;
};

}