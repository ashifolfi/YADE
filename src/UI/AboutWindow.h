
#ifndef YADE_ABOUTWIN
#define YADE_ABOUTWIN

#include <stdlib.h>

#include "EditorWindow.h"

namespace yade
{

class AboutWindow: public EditorWindow {
public:
	AboutWindow(bool is_open = false);
	void drawContents() override;
};
	
};

#endif