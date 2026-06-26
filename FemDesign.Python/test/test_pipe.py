import sys

import pytest

pytest.importorskip("win32pipe")
pytestmark = pytest.mark.skipif(
    sys.platform != "win32",
    reason="Requires Windows + pywin32 (Win32 named pipes) and a local FEM-Design installation.",
)

from femdesign.calculate.command import *
from femdesign.calculate.analysis import Analysis, CombSettings, Design
from femdesign.comunication import FemDesignConnection

def test_pipe():
    connection = FemDesignConnection(output_dir="test", minimized=True)
    assert connection.output_dir == os.path.join( os.getcwd(), "test" )

    connection._output_dir = None
    assert connection.output_dir == os.path.join( os.getcwd(), "FEM-Design API" )

    ## assert that connection.open() raises an error
    try:
        connection.Open("myModel.str")
    except Exception as e:
        assert isinstance(e, FileNotFoundError)
        assert str(e) == "File myModel.str not found"

    try:
        connection.Open("myModel.3dm")
    except Exception as e:
        assert isinstance(e, ValueError)
        assert str(e) == "file_name must have extension .struxml or .str"

    connection.__exit__()

def test_interaction_surface():
    connection = FemDesignConnection(minimized=True)
    connection.Open(r"test/assets/concrete_beam.struxml")
    guid = "c71d1619-420a-46fe-bbb7-423bf20fdcda"
    connection.GenerateInteractionSurface(guid, "test/assets/interaction_surface.txt", 0.5, True)

    assert os.path.exists("test/assets/interaction_surface.txt")
    assert os.path.getsize("test/assets/interaction_surface.txt") > 0

    os.remove("test/assets/interaction_surface.txt")